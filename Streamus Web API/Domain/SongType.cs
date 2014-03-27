using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Streamus_Web_API.Domain.Validators;

namespace Streamus_Web_API.Domain
{
    public enum SongType
    {
        None = 0,
        YouTube = 1,
        SoundCloud = 2
    }
}