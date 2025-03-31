using EventEdu.Domain.Entities;
using e = EventEdu.Domain.Entities;

namespace EventEdu.Application.DTOs;

public class CategoryCategoryDetailVM
{
	List<e::Category> Categories { get; set; }
	List<CategoryDetail> CategoryDetails { get; set; }
}
