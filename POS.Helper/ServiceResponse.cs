using System;
using System.Collections.Generic;

namespace POS.Helper
{
    public class ServiceResponse<T>
    {
        private ServiceResponse(int statusCode, List<string> errors)
        {
            StatusCode = statusCode;
            Errors = errors;
        }

        private ServiceResponse(Exception ex)
        {
            Errors = new List<string> { ex.Message.ToString() };
        }

        private ServiceResponse(int statusCode, T data)
        {
            StatusCode = statusCode;
            Data = data;
        }

        public bool Success
        {
            get
            {
                return Errors == null || Errors.Count == 0;
            }
        }

        public T Data { get; set; }
        public int StatusCode { get; set; } = 200;
        public List<string> Errors { get; set; } = new List<string>();

        public static ServiceResponse<T> ReturnException(Exception ex)
        {
            return new ServiceResponse<T>(ex);
        }

        public static ServiceResponse<T> ReturnFailed(int statusCode, List<string> errors)
        {
            return new ServiceResponse<T>(statusCode, errors);
        }

        public static ServiceResponse<T> ReturnFailed(int statusCode, string errorMessage)
        {
            return new ServiceResponse<T>(statusCode, new List<string> { errorMessage });
        }

        /// <summary>
        /// Returns the success.
        /// </summary>
        /// <returns></returns>
        public static ServiceResponse<T> ReturnSuccess()
        {
            return new ServiceResponse<T>(200, null);
        }

        /// <summary>
        /// Returns the result with Data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static ServiceResponse<T> ReturnResultWith200(T data)
        {
            return new ServiceResponse<T>(200, data);
        }

        /// <summary>
        /// Returns the Created Status code with data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static ServiceResponse<T> ReturnResultWith201(T data)
        {
            return new ServiceResponse<T>(201, data);
        }

        /// <summary>
        /// Returns the result with204.
        /// </summary>
        /// <returns></returns>
        public static ServiceResponse<T> ReturnResultWith204()
        {
            return new ServiceResponse<T>(204, null);
        }

        public static ServiceResponse<T> Return500()
        {
            return new ServiceResponse<T>(500, new List<string> { "An unexpected fault happened. Try again later." });
        }

        public static ServiceResponse<T> Return409(string message)
        {
            return new ServiceResponse<T>(409, new List<string> { message });
        }
        public static ServiceResponse<T> Return422(string message)
        {
            return new ServiceResponse<T>(422, new List<string> { message });
        }

        public static ServiceResponse<T> Return404()
        {
            return new ServiceResponse<T>(404, new List<string> { "Not Found" });
        }

        public static ServiceResponse<T> Return404(string message)
        {
            return new ServiceResponse<T>(404, new List<string> { message });
        }
    }

}
