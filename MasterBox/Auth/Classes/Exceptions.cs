using System;
using System.Runtime.Serialization;

/// Author: Goh Wen Liang (154473G) 

namespace MasterBox.Auth {
	[Serializable]
	internal class UserNotFoundException : Exception {
		public UserNotFoundException() {
		}

		public UserNotFoundException(string message) : base(message) {
		}

		public UserNotFoundException(string message, Exception innerException) : base(message, innerException) {
		}

		protected UserNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
	}

	[Serializable]
	internal class DatabaseUpdateFailureException : Exception {
		public DatabaseUpdateFailureException() {
		}

		public DatabaseUpdateFailureException(string message) : base(message) {
		}

		public DatabaseUpdateFailureException(string message, Exception innerException) : base(message, innerException) {
		}

		protected DatabaseUpdateFailureException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
	}

	[Serializable]
	internal class UserAlreadyExistsException : Exception {
		public UserAlreadyExistsException() {
		}

		public UserAlreadyExistsException(string message) : base(message) {
		}

		public UserAlreadyExistsException(string message, Exception innerException) : base(message, innerException) {
		}

		protected UserAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
	}

	[Serializable]
	internal class InvalidTOTPLength : Exception {
		public InvalidTOTPLength() {
		}

		public InvalidTOTPLength(string message) : base(message) {
		}

		public InvalidTOTPLength(string message, Exception innerException) : base(message, innerException) {
		}

		protected InvalidTOTPLength(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
	}


	[Serializable]
	internal class TooManyFailedLoginsException : Exception {
		public TooManyFailedLoginsException() {
		}

		public TooManyFailedLoginsException(string message) : base(message) {
		}

		public TooManyFailedLoginsException(string message, Exception innerException) : base(message, innerException) {
		}

		protected TooManyFailedLoginsException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
	}

}