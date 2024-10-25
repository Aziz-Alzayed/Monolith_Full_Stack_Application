using AutoMapper;
using FSTD.Application.DTOs.Productivity.Tasks;
using FSTD.DataCore.Models.ProductivityModels;

namespace FSTD.Infrastructure.Mapping.Productivity
{
    public class TaskMappingProfile : Profile
    {
        public TaskMappingProfile()
        {
            CreateMap<TasksModel, TaskDto>().ReverseMap();
        }
    }
}
