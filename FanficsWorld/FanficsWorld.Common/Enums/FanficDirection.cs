using System.ComponentModel.DataAnnotations;

namespace FanficsWorld.Common.Enums;

public enum FanficDirection
{
    [Display(Name = "Gen")]
    Gen,
    [Display(Name = "Het")]
    Het,
    [Display(Name = "Slash")]
    Slash,
    [Display(Name = "Femslash")]
    FemSlash,
    [Display(Name = "Other")]
    Other,
    [Display(Name = "Mixed")]
    Mixed,
    [Display(Name = "Article")]
    Article
}