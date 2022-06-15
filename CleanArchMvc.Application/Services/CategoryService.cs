using AutoMapper;
using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using CleanArchMvc.Domain.Entities;
using CleanArchMvc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchMvc.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task Add(CategoryDTO categoryDTO)
        {
            var categoryEntitiy = _mapper.Map<Category>(categoryDTO);
            await _categoryRepository.Create(categoryEntitiy);
        }

        public async Task<CategoryDTO> GetById(int? id)
        {
            var categoryEntitiy = await _categoryRepository.GetById(id);
            return _mapper.Map<CategoryDTO>(categoryEntitiy);
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategories()
        {
            var categoryEntitiy = await _categoryRepository.GetCategories();
            return _mapper.Map<IEnumerable<CategoryDTO>>(categoryEntitiy);
        }

        public async Task Remove(int? id)
        {
            var categoryEntitiy = _categoryRepository.GetById(id).Result;
            await _categoryRepository.Remove(categoryEntitiy);
        }

        public async Task Update(CategoryDTO categoryDTO)
        {
            var categoryEntitiy = _mapper.Map<Category>(categoryDTO);
            await _categoryRepository.Update(categoryEntitiy);
        }
    }
}
