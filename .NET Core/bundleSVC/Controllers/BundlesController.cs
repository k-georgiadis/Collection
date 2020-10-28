using AutoMapper;
using bundleSVC.Data;
using bundleSVC.DTO;
using bundleSVC.Models;
using BundleSVC.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace bundleSVC.Controllers
{
    [Route("api/bundles")]
    [ApiController]
    public class BundlesController : Controller
    {
        private readonly IBundleRepo _repo;
        private readonly IMapper _mapper;

        public BundlesController(IBundleRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        //GET api/bundles
        /// <summary>
        /// Returns all available bundles.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Bundle> GetAllBundles()
        {
            var res = _repo.GetAllBundles();
            return Ok(_mapper.Map<IEnumerable<BundleReadDTO>>(res));
        }

        //GET api/bundles/{id}
        /// <summary>
        /// Get a specific bundle by id/code.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetBundleByCode")]
        public ActionResult<BundleReadDTO> GetBundleByCode([FromRoute]int id)
        {
            var res = _repo.GetBundleByCode(id);

            if (res == null)
                return NotFound();
            else
                return Ok(_mapper.Map<BundleReadDTO>(res));
        }

        //GET api/bundles/GetBundleByName/{name}
        /// <summary>
        /// Get bundles by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]/{name}")]
        public ActionResult<IEnumerable<BundleReadDTO>> GetBundleByName([FromRoute]string name)
        {
            var res = _repo.GetBundleByName(name);

            if (res == null || !res.Any())
                return NotFound();
            else
                return Ok(_mapper.Map<IEnumerable<BundleReadDTO>>(res));
        }

        //GET api/bundles/GetBundleByPrice/{price}/{condition}/{order}
        /// <summary>
        /// Get bundles filtered by price in ascending or descending order.
        /// The order is optional when using the equality operator "==".
        /// </summary>
        /// <param name="price">        The price to check.                                            </param>
        /// <param name="condition">    Smaller than or equal to, Greater than or equal to, Equal to.  </param>
        /// <param name="order">        Ascending ("ASC"), Descending ("DESC").                        </param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]/{price}/{condition}")] //For "==" requests only.
        [Route("[action]/{price}/{condition}/{order}")]
        public ActionResult<IEnumerable<BundleReadDTO>> GetBundleByPrice([FromRoute]float price, [FromRoute]string condition, [FromRoute]string order)
        {
            var res = _repo.GetBundleByPrice(price, condition, order);

            if (res == null || !res.Any())
                return NotFound();
            else
                return Ok(_mapper.Map<IEnumerable<BundleReadDTO>>(res));
        }

        //POST api/bundles/AddBundle
        /// <summary>
        /// Add a new bundle to the database.
        /// </summary>
        /// <param name="bundleDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public ActionResult<BundleReadDTO> AddBundle([FromBody]BundleAddDTO bundleDTO)
        {
            var bundleModel = _mapper.Map<Bundle>(bundleDTO);

            if (bundleModel == null)
                return NotFound();

            //Create bundle and update database.
            _repo.AddBundle(bundleModel);
            _repo.SaveChanges();

            var bundleReadDTO = _mapper.Map<BundleReadDTO>(bundleModel);

            //Return 201 HTTP code.
            return CreatedAtRoute(nameof(GetBundleByCode), new { id = bundleReadDTO.B_code }, bundleReadDTO);
        }

        //PATCH api/bundles/DeactivateBundle/{id}
        /// <summary>
        /// De-activate a specific bundle by id/code.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("[action]/{id}")]
        public ActionResult<BundleUpdateDTO> DeactivateBundle([FromRoute]int id)
        {
            var res = _repo.GetBundleByCode(id);

            //Check if bundle exists.
            if (res == null)
                return NotFound();

            //Deactivate bundle.
            res.B_active = false;

            var bundle = _mapper.Map<BundleUpdateDTO>(res);

            _mapper.Map(bundle, res);
            _repo.SaveChanges();

            return Ok();
        }

        //PATCH api/bundles/{id}
        /// <summary>
        /// Update a bundle by id/code.
        /// The operation, field and value are specified in the body as a JSON string.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{id}")]
        public ActionResult<BundleUpdateDTO> UpdateBundle([FromRoute]int id, [FromBody]JsonPatchDocument<BundleUpdateDTO> patchDoc)
        {
            var res = _repo.GetBundleByCode(id);

            //Check if bundle exists.
            if (res == null)
                return NotFound();

            //Update bundle.
            var bundle = _mapper.Map<BundleUpdateDTO>(res);
            patchDoc.ApplyTo(bundle, ModelState);

            if (!TryValidateModel(bundle))
                return ValidationProblem(ModelState);

            _mapper.Map(bundle, res);
            _repo.UpdateBundle(res); //Does nothing for now.
            _repo.SaveChanges();

            return Ok();
        }

        //DELETE api/bundles/{id}
        /// <summary>
        /// Remove a bundle, from the database, by id/code.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult RemoveBundle([FromRoute]int id)
        {
            var res = _repo.GetBundleByCode(id);

            //Check if bundle exists.
            if (res == null)
                return NotFound();

            //Delete bundle from database.
            _repo.DeleteBundle(res);
            _repo.SaveChanges();

            return NoContent();
        }
    }
}
