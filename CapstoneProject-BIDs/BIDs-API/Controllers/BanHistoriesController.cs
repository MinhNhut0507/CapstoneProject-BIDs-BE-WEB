﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data_Access.Entities;
using Business_Logic.Modules.BanHistoryModule.Interface;
using Business_Logic.Modules.BanHistoryModule.Request;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanHistoriesController : ControllerBase
    {
        private readonly IBanHistoryService _BanHistoryService;

        public BanHistoriesController(IBanHistoryService BanHistoryService)
        {
            _BanHistoryService = BanHistoryService;
        }

        // GET api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BanHistory>>> GetBanHistorysForAdmin()
        {
            try
            {
                var response = await _BanHistoryService.GetAll();
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BanHistory>> GetBanHistoryByUserID([FromRoute] Guid? id)
        {
            var BanHistory = await _BanHistoryService.GetBanHistoryByUserID(id);

            if (BanHistory == null)
            {
                return NotFound();
            }

            return BanHistory;
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_name/{name}")]
        public async Task<ActionResult<BanHistory>> GetBanHistoryByUserName([FromRoute] string name)
        {
            var BanHistory = await _BanHistoryService.GetBanHistoryByUserName(name);

            if (BanHistory == null)
            {
                return NotFound();
            }

            return BanHistory;
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutBanHistory([FromBody] UpdateBanHistoryRequest updateBanHistoryRequest)
        {
            try
            {
                await _BanHistoryService.UpdateBanHistory(updateBanHistoryRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BanHistory>> PostBanHistory([FromBody] CreateBanHistoryRequest createBanHistoryRequest)
        {
            try
            {
                return Ok(await _BanHistoryService.AddNewBanHistory(createBanHistoryRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //// DELETE api/<ValuesController>/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteBanHistory([FromRoute] Guid? id)
        //{
        //    try
        //    {
        //        await _BanHistoryService.d(id);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}