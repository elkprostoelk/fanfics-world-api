using System.ComponentModel.DataAnnotations;

namespace FanficsWorld.Common.Enums;

public enum FanficOrigin
{
    [Display(Name = "Original Text")]
    OriginalText,
    [Display(Name = "Translation")]
    Translation
}