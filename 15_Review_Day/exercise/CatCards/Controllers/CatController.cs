using System.Collections.Generic;
using CatCards.DAO;
using CatCards.Models;
using CatCards.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatCards.Controllers
{
    [Route("cards")]
    [ApiController]

    public class CatController : ControllerBase
    {
        private readonly ICatCardDao cardDao;
        private readonly ICatFactService catFactService;
        private readonly ICatPicService catPicService;

        public CatController(ICatCardDao _cardDao, ICatFactService _catFact, ICatPicService _catPic)
        {
            catFactService = _catFact;
            catPicService = _catPic;
            cardDao = _cardDao;
        }

        //Not correct, not sure how to pull fact and pic and combine into random card.
        [HttpGet("random")]
        public ActionResult<CatCard> GetRandomCard(int id)
        {
            CatCard returned = cardDao.GetCard(id);

            return returned;
        }

        [HttpGet()]
        public ActionResult<List<CatCard>> GetCards()
        {

            return Ok(cardDao.GetAllCards());
        }

        [HttpGet("{id}")]
        public ActionResult<CatCard> GetCard(int id)
        {
            CatCard card = cardDao.GetCard(id);

            if (card != null)
            {
                return card;
            }

            return NotFound();
        }

        [HttpPost()]
        public ActionResult<CatCard> SaveCard(CatCard catCard)
        {
            CatCard added = cardDao.SaveCard(catCard);
            return Created($"/cards/{added.CatCardId}", added);
        }

        [HttpPut("{id}")]
        public ActionResult<CatCard> UpdatedCard(int id, CatCard catCard)
        {
            CatCard cardToUpdate = cardDao.GetCard(id);
            if (cardToUpdate == null)
            {
                return NotFound();
            }
            if (id != cardToUpdate.CatCardId)
            {
                return BadRequest("Cat card ID did not match supplied URL");
            }

            return Ok(cardDao.UpdateCard(catCard)); 
        }


        [HttpDelete("{id}")]
        public ActionResult DeleteCard(int id)
        {
            bool result = cardDao.RemoveCard(id);
            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
