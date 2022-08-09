using Imkery.API.Client.Core;
using Imkery.Data.Storage.Core;
using Imkery.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imkery.Server.Controllers
{
    public abstract class BaseController<T, K> : ControllerBase
        where T : EFRepository<K>
        where K : class, IEntity<K>, new()
    {
        protected EFRepository<K> _repository;
        public BaseController(EFRepository<K> repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public virtual async Task<ActionResult<K>> AddAsync([FromBody] K entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addedEntity = await _repository.AddAsync(entity);
            return Ok(addedEntity);
        }

        [HttpDelete]
        public virtual async Task<ActionResult<bool>> DeleteAsync([FromBody] K entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entityFromDb = await _repository.GetItemByIdAsync(entity.Id);
            if (entityFromDb == null)
            {
                return NotFound();
            }
            return Ok(await _repository.DeleteAsync(entityFromDb));
        }

        [HttpGet("")]
        public virtual async Task<ActionResult<ICollection<K>>> GetCollectionAsync([FromQuery(Name = "filterpaging")] string filterPagingOptionsJson)
        {
            ICollection<K> collection = await InternalGetCollectionAsync(filterPagingOptionsJson);
            return Ok(collection);
        }

        protected virtual async Task<ICollection<K>> InternalGetCollectionAsync(string filterPagingOptionsJson)
        {
            FilterPagingOptions filterPagingOptions = FilterPagingOptions.FromString(filterPagingOptionsJson);
            string sortField = null;
            bool desc = false;
            if (!string.IsNullOrWhiteSpace(filterPagingOptions.SortProperty))
            {
                string[] parts = filterPagingOptions.SortProperty.Split(';');
                sortField = parts[0];
                desc = parts[1] == "Down";
            }
            var collection = await _repository.GetCollectionAsync((filterPagingOptions.Page) * filterPagingOptions.ItemsPerPage, filterPagingOptions.ItemsPerPage, sortField, desc, filterPagingOptions.FilterParameters, filterPagingOptions.Includes);
            return collection;
        }

        [HttpGet("count")]
        public virtual async Task<ActionResult<int>> GetCountAsync([FromQuery(Name = "filterpaging")] string filterPagingOptionsJson)
        {
            FilterPagingOptions filterPagingOptions = FilterPagingOptions.FromString(filterPagingOptionsJson);
            return Ok(await _repository.GetCountAsync(filterPagingOptions.FilterParameters));
        }
        //
        [HttpGet("{id}")]
        public virtual async Task<ActionResult<K>> GetItemByIdAsync(Guid id)
        {
            return Ok(await _repository.GetItemByIdAsync(id));
        }


        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> DeleteItemByIdAsync(Guid id)
        {
            K entity = await _repository.GetItemByIdAsync(id);
            if (entity == null)
            {
                return BadRequest();
            }
            await _repository.DeleteAsync(entity);
            return Ok();
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<K>> UpdateAsync(Guid id, [FromBody] K entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entityFromDb = await _repository.GetItemByIdAsync(id);
            if (entityFromDb.OwnerId != entity.OwnerId)
            {
                return BadRequest("Can not change owner this way");
            }
            if (entityFromDb == null)
            {
                return NotFound();
            }
            return Ok(await _repository.UpdateAsync(id, entity));
        }
    }
}
