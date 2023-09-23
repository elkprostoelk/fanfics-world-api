using System.ComponentModel.DataAnnotations;

namespace FanficsWorld.Common.Enums;

public enum FanficRating
{
    [Display(Name = "G")]
    G,
    [Display(Name = "PG-13")]
    Pg13,
    [Display(Name = "R")]
    R,
    [Display(Name = "NC-17")]
    Nc17,
    [Display(Name = "NC-21")]
    Nc21
}