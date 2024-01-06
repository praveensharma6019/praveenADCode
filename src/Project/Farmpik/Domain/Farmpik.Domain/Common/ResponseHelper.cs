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
    public static class ResponseHelper
    {
        public static ApiResponse<T> GetResponse<T>(T payload, bool status = true,
            string successMes = ResponseMessage.Success, string errorMes = ResponseMessage.Error,
            int? statusCode = ResponseStatusCode.NoContent)
        {
            ApiResponse<T> response = new ApiResponse<T>();
            return status ? response.SetResponse(payload, successMes) :
                response.SetResponse(default, errorMes, status, statusCode);
        }

        public static PaginationApiResponse<T> GetPaginationResponse<T>(T payload, bool status = true,
         string successMes = ResponseMessage.Success, string errorMes = ResponseMessage.No_Record_Found,
         int? statusCode = ResponseStatusCode.Success, long totalRecord = 0)
        {
            PaginationApiResponse<T> response = new PaginationApiResponse<T>
            {
                Count = totalRecord
            };
            if (status)
            {
                response.SetResponse(payload, successMes);
            }
            else
            {
                response.SetResponse(payload, errorMes, status, statusCode);
            }

            return response;
        }
    }
}