﻿using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    /// <summary>
    /// Service record.
    /// </summary>
    public class FileCabinetRecord
    {
        /// <summary>
        /// Gets or sets record ID.
        /// </summary>
        /// <value>
        /// Record ID.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets record FirstName.
        /// </summary>
        /// <value>
        /// Record FirstName.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets record LastName.
        /// </summary>
        /// <value>
        /// Record LastName.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets record DateOfBirth.
        /// </summary>
        /// <value>
        /// Record DateOfBirth.
        /// </value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets record Personal Account Type.
        /// </summary>
        /// <value>
        /// Record Personal Account Type.
        /// </value>
        public char Type { get; set; }

        /// <summary>
        /// Gets or sets record Personal Account Number.
        /// </summary>
        /// <value>
        /// Record Personal Account Number.
        /// </value>
        public short Number { get; set; }

        /// <summary>
        /// Gets or sets record Personal Account Balance.
        /// </summary>
        /// <value>
        /// Record Personal Account Balance.
        /// </value>
        public decimal Balance { get; set; }
    }
}
