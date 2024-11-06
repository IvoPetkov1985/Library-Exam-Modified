﻿namespace Library.Data.Common
{
    public static class DataConstants
    {
        // Book:
        public const int BookTitleMinLength = 10;
        public const int BookTitleMaxLength = 50;

        public const int BookAuthorMinLength = 5;
        public const int BookAuthorMaxLength = 50;

        public const int BookDescriptionMinLength = 5;
        public const int BookDescriptionMaxLength = 5000;

        public const decimal BookRatingMin = 0;
        public const decimal BookRatingMax = 10;

        // Category:
        public const int CategoryNameMinLength = 5;
        public const int CategoryNameMaxLength = 50;

        public const string MissingCategoryErrorMsg = "This category does not exist.";
    }
}