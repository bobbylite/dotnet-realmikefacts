// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace general.purpose.poc.Models.Web;

/// <summary>
/// View model for errors.
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// Gets or sets a request id.
    /// </summary>
    /// <value><see cref="string"/>.</value>
    public string? RequestId { get; set; }

    /// <summary>
    /// Gets a value indicating whether the UI should display request id.
    /// </summary>
    /// <returns><see cref="bool"/>.</returns>
    public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);
}
