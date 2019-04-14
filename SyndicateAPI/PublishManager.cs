using SyndicateAPI.BusinessLogic.Interfaces;
using System;
using System.Linq;
using System.Timers;

namespace SyndicateAPI
{
    public class PublishManager
    {
        private IPostService PostService { get; set; }

        public PublishManager()
        {
            var timer = new Timer();

            timer.Elapsed += new ElapsedEventHandler(PublishPosts);
            timer.Interval = 60000 * 5;
            timer.Start();
        }

        public void SetServices(IPostService postService)
        {
            if (PostService == null)
                PostService = postService;
        }

        public void PublishPosts(object source, ElapsedEventArgs e)
        {
            var posts = PostService.GetAll()
                .Where(x => !x.IsPublished)
                .ToList();

            foreach (var post in posts)
            {
                if (post.PublishTime <= DateTime.UtcNow && !post.IsPublished)
                {
                    post.IsPublished = true;
                    PostService.Update(post);
                }
            }
        }
    }
}
