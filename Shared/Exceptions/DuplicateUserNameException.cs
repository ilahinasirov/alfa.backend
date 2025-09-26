using Shared.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Exceptions
{
    public class NotFoundEntityException()
        : BaseException(SharedResources.ResourceManager.GetString("not_found_entity"),1);
    public class DuplicateUserNameException()
    : BaseException(SharedResources.ResourceManager.GetString("duplicate_username"),2);
    public class DuplicateDataException()
    : BaseException(SharedResources.ResourceManager.GetString("duplicate_data_exception"),3);
    public class DuplicateUserException()
    : BaseException(SharedResources.ResourceManager.GetString("duplicate_user"),4);




    public class DuplicateSystemConstantException()
        : BaseException(SharedResources.ResourceManager.GetString("duplicate_system_constant"),15);
    public class IncorrectCurrentPasswordException()
       : BaseException(SharedResources.ResourceManager.GetString("current_password_incorrect"),17);
    public class DontMatchPasswordException()
       : BaseException(SharedResources.ResourceManager.GetString("passwords_dont_match"),18);
    public class InvalidPasswordFormatException()
       : BaseException(SharedResources.ResourceManager.GetString("invalid_password_exception"),20);

    public class ProfileInformationsNotCompleted()
       : BaseException(SharedResources.ResourceManager.GetString("profile_infos_not_complited"),21);

    public class InvalidFileStrorage()
      : BaseException(SharedResources.ResourceManager.GetString("invalid_file_storage"),22);


    public class OtpExpiredException()
     : BaseException(SharedResources.ResourceManager.GetString("error_otp_expired"), 24);
    public class OtpAttemptsExceedException()
     : BaseException(SharedResources.ResourceManager.GetString("error_otp_attempts_exceed"), 25);
    public class OtpVerificationException()
     : BaseException(SharedResources.ResourceManager.GetString("error_otp_verification"), 26);
    public class OtpExpiredOrVerificationException()
     : BaseException(SharedResources.ResourceManager.GetString("error_otp_verification_or_expired"), 27);
    public class DontExistDataAtRedisException()
     : BaseException(SharedResources.ResourceManager.GetString("error_dont_exist_data_in_redis"), 28);
    public class DontExistEmailDataAtRedisException()
    : BaseException(SharedResources.ResourceManager.GetString("error_dont_exist_email_data_in_redis"), 29);

    public class DuplicateFinException()
   : BaseException(SharedResources.ResourceManager.GetString("duplicate_fin"), 30);

    public class DuplicateEmailException()
: BaseException(SharedResources.ResourceManager.GetString("duplicate_email"), 31);
    public class DuplicateFinAndEmailException()
: BaseException(SharedResources.ResourceManager.GetString("duplicate_fin_and_email"), 32);
    public class RequiredOtpTimeException()
: BaseException(SharedResources.ResourceManager.GetString("required_otp_time_exception"), 33);

}
