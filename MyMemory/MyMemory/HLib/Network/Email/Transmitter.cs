// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Transmitter.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Transmits an email to all of it's recepiants asynchronously.
//   Updates Email.Recepiant.TransmitState upponn transmittion
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Network.Email
{

    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Threading;
    using System.Threading.Tasks;

    using HLib.Logging;


    /// <summary>
    /// Transmits an email to all of it's recepiants asynchronously.
    /// Updates Email.Recepiant.TransmitState upponn transmittion
    /// </summary>
    public class Transmitter : IEmailTransmitter
    {

        private readonly BlockingCollection<IEmail> _requestQueue;
        private readonly ILoggable _logger;
        private readonly CancellationTokenSource _exitThreadEvent;


        public Transmitter(BlockingCollection<IEmail> requestQueue, ILoggable logger)
        {
            try
            {
                _logger = logger;
                _requestQueue = requestQueue;
                _exitThreadEvent = new CancellationTokenSource();

                new Thread(TransmitterThread).Start(new TransmitterThreadData { Logger = logger, Queue = requestQueue, ExitCancelationToken = _exitThreadEvent.Token });
            }
            catch (Exception ex)
            {
                logger.LogException(ex);
            }
        }


        public bool Transmit(IEmail email)
        {
            try
            {
                Parallel.ForEach(
                    email.RecepiantSettings.RecepiantList,
                    recepiant => { recepiant.TransmitState = TransmitState.Pending; });

                _requestQueue.Add(email);
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogException(exception);
            }

            return false;
        }


        public void Dispose()
        {
            _exitThreadEvent.Cancel();
        }


        private static void TransmitterThread(object state)
        {
            var data = state as TransmitterThreadData;
            if (data == null)
            {
                System.Diagnostics.Trace.WriteLine("TransmitterThread failed to cast input state to TransmitterThreadData.");
                return;
            }

            var logger = data.Logger ?? Loggers.Null;

            var queue = data.Queue;
            if (queue == null)
            {
                logger.LogError("TransmitterThread: Given queue is null.");
                return;
            }

            while (true)
            {
                try
                {
                    IEmail info;

                    try
                    {
                        if (!queue.TryTake(out info, Timeout.Infinite, data.ExitCancelationToken))
                        {
                            queue.Dispose();
                            break;
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // UNDONE:
                        // Can't believe this! THIS IS RIDICULOUS! THE ONLY WAY
                        // TO RELEASE A TAKE IS AN EXCEPTION WHILE NO ERROR HAS
                        // OCCURED. SUCH STUPID DESIGN ENGINEERS(?) GET A JOB AT
                        // A COMPANY LIKE  (( MICROSOFT )) AND I HAVE TO SEEK
                        // JOB FOR 6 MONTHS, IN JUST ANY POSSIBLE COMPANY TO BE
                        // ABLE TO LIVE IN A FREE COUNTRY.
                        // GOD DAMN MEDIA FOR LABELS THEY ATTACHED TO ME WITH MY BIRTH.
                        // [IRANIAN] = LESS HUMAN BEING, LESS HUMAN RIGHTS, BOTH INSIDE
                        // THE COUNTRY *AND* OUTSIDE. :/

                        // Shall exit thread.
                        queue.Dispose();
                        break;
                    }
                    catch (ObjectDisposedException)
                    {
                        break;
                    }
                    catch (InvalidOperationException)
                    {
                        // Add complete was called or some other issue. Let's attempt 
                        // taking again untileiteher the problem is recovered or
                        // another exception stopps us
                        continue;
                    }

                    if (info == null) continue;

                    foreach (var recepiant in info.RecepiantSettings.RecepiantList)
                    {
                        try
                        {
                            SendEmail(recepiant, info.SenderSettings, info.ServerSettings, info.Message);
                        }
                        catch (Exception exception)
                        {
                            logger.LogException(exception);
                            if (recepiant != null)
                            {
                                recepiant.TransmitState = TransmitState.SendFailed;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogException(ex);
                }
            }
        }


        private static void SendEmail(IRecepiant recepiant, ISenderSettings senderSettings, IServerSettings serverSettings, IMessage message)
        {

            using (var smtp = new SmtpClient(serverSettings.Address, serverSettings.Port)
            {
                Credentials = new NetworkCredential(senderSettings.Address, senderSettings.Password),
                EnableSsl = serverSettings.SupportsSsl
            })
            {
                recepiant.TransmitState = TransmitState.Sending;

                // Testing animation
//#if DEBUG
//                Thread.Sleep(1000);
//                recepiant.TransmitState = TransmitState.SendFailed;
//                return;
//#endif

                using (var email = new MailMessage())
                {
                    // set sender info
                    email.From = new MailAddress(senderSettings.Address, senderSettings.Name);

                    // set mail message
                    email.IsBodyHtml = false;
                    email.Subject = message.Subject;
                    email.To.Add(recepiant.Address);
                    email.Body = message.Body;

                    // Add selected attachments
                    var attachments = message.Attachments.Where(attachment => attachment.IsIncluded);
                    foreach (var attachment in attachments)
                    {
                        email.Attachments.Add(new Attachment(attachment.FilePath));
                    }

                    // Send email
                    smtp.Send(email);

                    // Cleanup
                    foreach (var item in email.Attachments)
                    {
                        item.Dispose();
                    }

                    email.Attachments.Clear();

                    // Update original mail state
                    recepiant.TransmitState = TransmitState.SendSuccessful;
                }
            }
        }
    }
}
