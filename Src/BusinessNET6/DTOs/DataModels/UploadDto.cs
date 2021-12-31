using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DataModels
{
    public class UploadDto 
    {
        public List<ExecutionPlanDto> ExecutionPlan { get; set; } = new();
        public List<FormHeaderDto> FormHeader { get; set; } = new();
        public List<OrganizationUintDto> OrganizationUint { get; set; } = new();
        public List<GroupMasterDto> GroupMaster { get; set; } = new();
        public List<FileRepositoryDto> FileRepository { get; set; } = new();
    }
}
