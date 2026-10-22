using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Models;

namespace SurveyBasket.API.Services
{
    public interface IPollService
    {
        public ICollection<Poll> GetAll();
        public Poll? Get(int id);
        public Poll Add(Poll poll);
        public bool Update(int id, Poll poll);

        public bool Delete(int id);



    }
}
