using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Models;

namespace SurveyBasket.API.Services
{
    public class PollService : IPollService
    {
        private readonly List<Poll> _polls = new List<Poll>()
        {
           new Poll
           {
               Id = 1,
               Name = "Ali",
               Description = "Eng",
           } ,
           new Poll
           {
               Id = 2,
               Name = "two",
               Description = " fdf "
           }
        };

        public ICollection<Poll> GetAll()
        {
            return _polls;
        }

        public Poll? Get(int id)
        {
           return _polls.SingleOrDefault(x => x.Id == id);
           
        }

        public Poll Add (Poll poll)
        {
            _polls.Add(poll);
            return poll;
        }

        public bool Update(int id, Poll poll)
        {
           var currnetPoll = Get(id);

            if (currnetPoll == null)
                return false;

            currnetPoll.Id = id;
            currnetPoll.Name = poll.Name;
            currnetPoll.Description = poll.Description;

            return true;

        }

        public bool Delete(int id)
        {
            Poll poll = Get(id);

            if (poll == null)
                return false;

            _polls.Remove(poll);
            return true;
        }
    }
}
