using System.Collections.Generic;
using CatCards.DAO;
using CatCards.Models;
using CatCards.Services;
using Microsoft.AspNetCore.Mvc;
using CatCards.DAO;
using CatCards.Models;


namespace CatCards.Controllers
{
    [Route("[controller]")]
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
        [HttpGet("/api/cards/random")]
        public ActionResult<CatCard> getRandomCat()
        {
            CatCard newCatCard = new CatCard();
            newCatCard.CatFact = catFactService.GetFact().Text;
            newCatCard.ImgUrl = catPicService.GetPic().File;


            return newCatCard;
        }

        [HttpGet("/api/cards")]
        public ActionResult<List<CatCard>> GetCards()
        {

            return Ok(cardDao.GetAllCards());
        }

        [HttpGet("/api/cards/{catCardId}")]
        public ActionResult<CatCard> getCat(int catCardId)
        {
            CatCard card = cardDao.GetCard(catCardId);

            if (card != null)
            {
                return Ok(card);
            }

            return NotFound();
        }

        [HttpPost("/api/cards")]
        public ActionResult<CatCard> SaveCard(CatCard catCard)
        {
            CatCard added = cardDao.SaveCard(catCard);
            return Created($"/cards/{added.CatCardId}", added);
        }

        [HttpPut("/api/cards/{id}")]
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


        [HttpDelete("/api/cards/{id}")]
        public ActionResult DeleteCard(int id)
        {
            CatCard cardToRemove = cardDao.GetCard(id);
           
            if (cardToRemove == null)
            {
                return NotFound();
            }

            bool result = cardDao.RemoveCard(id);
            if (result)
            {

                return NoContent();
            }
            
            return StatusCode(500);
        }
    }
}
