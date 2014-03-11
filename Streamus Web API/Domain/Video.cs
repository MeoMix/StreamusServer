using System;
using FluentValidation;
using Streamus_Web_API.Domain.Interfaces;
using Streamus_Web_API.Domain.Validators;
using Streamus_Web_API.Dto;

namespace Streamus_Web_API.Domain
{
    public class Video : IAbstractDomainEntity<string>
    {
        public virtual string Id { get; set; }
        public virtual string Title { get; set; }
        public virtual int Duration { get; set; }
        public virtual string Author { get; set; }
        public virtual bool HighDefinition { get; set; }

        public Video()
        {
            Id = string.Empty;
            Title = string.Empty;
            Author = string.Empty;
        }

        public Video(string id, string title, int duration, string author)
        {
            Id = id;
            Title = title;
            Duration = duration;
            Author = author;
        }

        //  TODO: Consider not coupling to Dto here and just pass in params or use constructor.
        public static Video Create(VideoDto videoDto)
        {
            Video video = new Video(videoDto.Id, videoDto.Title, videoDto.Duration, videoDto.Author);
            return video;
        }

        public virtual void ValidateAndThrow()
        {
            var validator = new VideoValidator();
            validator.ValidateAndThrow(this);
        }

        public override int GetHashCode()
        {
            bool thisIsTransient = Equals(Id, string.Empty);

            if (thisIsTransient)
            {
                throw new ApplicationException("Video should never be transient.");
            }

            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Video other = obj as Video;
            if (other == null)
                return false;

            // handle the case of comparing two NEW objects
            bool otherIsTransient = Equals(other.Id, string.Empty);
            bool thisIsTransient = Equals(Id, string.Empty);
            if (otherIsTransient || thisIsTransient)
            {
                throw new ApplicationException("Video should never be transient.");
            }

            return other.Id.Equals(Id);
        }
    }
}