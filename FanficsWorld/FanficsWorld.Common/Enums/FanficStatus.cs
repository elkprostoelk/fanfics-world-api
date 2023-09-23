using System.ComponentModel.DataAnnotations;

namespace FanficsWorld.Common.Enums;

public enum FanficStatus
{
    [Display(Name = "In progress")]
    InProgress,
    [Display(Name = "Finished")]
    Finished,
    [Display(Name = "Frozen")]
    Frozen
}