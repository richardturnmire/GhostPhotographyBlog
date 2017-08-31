using GhostPhotographerBlog.Data.Interfaces;
using GhostPhotographerBlog.Data.Repositories;
using System;

namespace GhostPhotographerBlog.Data.Factory
{
    public class BlogRespositoryFactory
    {
        public static IBlogRepository GetMode()
        {
            switch (Settings.GetMode())
            {
                case "Test":
                    Settings.SetConnectionString("TestingConnection");
                    return new BlogRepository();
                case "Prod":
                    Settings.SetConnectionString("DefaultConnection");
                    return new BlogRepository();
                default:
                    throw new Exception("Could not find valid Mode in Web.config file.");
            }
        }
    }
}
