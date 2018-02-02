using Grace.DependencyInjection;
using MyMemory.Domain;
using MyMemory.Domain.Abstract;


namespace MyMemory
{

    public class BootStrapper
    {

        private DependencyInjectionContainer _container;


        public void Bootstrap()
        {
            _container = new DependencyInjectionContainer();

            _container.Configure(c =>
            {
                c.ExportInstance(CreatePlaylistContainerStore()).As<IPlaylistContainerStore>().Lifestyle.Singleton();
            });
        }


        public T Locate<T>()
        {
            return _container.Locate<T>();
        }


        private static IPlaylistContainerStore CreatePlaylistContainerStore()
        {
            return new PlaylistContainerFileStore(
                System.IO.Path.Combine(
                    HLib.Io.PathUtil.ApplicationDirectory,
                    "playlist.json"),
                new JsonStringSerializer());
        }

    }
}
