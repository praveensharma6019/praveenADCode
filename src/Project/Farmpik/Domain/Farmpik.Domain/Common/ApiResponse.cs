/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using static Farmpik.Domain.Common.ResponseConstants;

namespace Farmpik.Domain.Common
{
    public class ApiResponse<T>
    {
        public bool? Status { get; set; }
        public int? StatusCode { get; set; }
        public string Message { get; set; }
        public T Payload { get; set; }

        public ApiResponse<T> SetResponse(T payload, string message = ResponseMessage.Success,
            bool? status = true, int? statusCode = ResponseStatusCode.Success)
        {
            Status = status;
            StatusCode = statusCode;
            Message = message;
            Payload = payload;

            return this;
        }
    }
}