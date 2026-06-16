using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Modules.Documents.Application.DTOs;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Modules.Documents.Application.Services;

public class TagService : ITagService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public TagService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<List<TagDto>> GetAllAsync()
    {
        var tags = await _unitOfWork.Tags.GetAllAsync();
        return _mapper.Map<List<TagDto>>(tags);
    }

    public async Task<TagDto?> GetByIdAsync(Guid id)
    {
        var tag = await _unitOfWork.Tags.GetByIdAsync(id);
        return tag is null ? null : _mapper.Map<TagDto>(tag);
    }

    public async Task<TagDto> CreateAsync(CreateTagDto dto)
    {
        var tag = _mapper.Map<Tag>(dto);
        tag.Slug = Slugify(string.IsNullOrWhiteSpace(dto.Slug) ? dto.Name : dto.Slug!);
        tag.CreatedAt = _dateTime.UtcNow;

        await _unitOfWork.Tags.CreateAsync(tag);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<TagDto>(tag);
    }

    public async Task<TagDto?> UpdateAsync(Guid id, UpdateTagDto dto)
    {
        var tag = await _unitOfWork.Tags.GetByIdAsync(id);
        if (tag is null)
        {
            return null;
        }

        _mapper.Map(dto, tag);
        tag.Slug = Slugify(string.IsNullOrWhiteSpace(dto.Slug) ? dto.Name : dto.Slug!);
        tag.UpdatedAt = _dateTime.UtcNow;

        _unitOfWork.Tags.Update(tag);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<TagDto>(tag);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.Tags.DeleteAsync(id);
        if (!deleted)
        {
            return false;
        }

        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private static string Slugify(string value) =>
        value.Trim().ToLowerInvariant().Replace(" ", "-");
}
