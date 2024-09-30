using System.ComponentModel;

namespace CTMS.Core
{
    /// <summary>
    /// Summary description for CommonMessage
    /// </summary>
    public enum CommonMessage
    {
        [Description("Record created successfully.")]
        Created = 1,

        [Description("Record updated successfully.")]
        Updated = 2,

        [Description("Record deleted successfully.")]
        Deleted = 3,

        [Description("Record Already exists.")]
        AlreadyExists = 4,

        [Description("Dependency record exists.")]
        DependencyRecord = 5,

        [Description("Record already deleted.")]
        RecordAlreadyDeleted = 6,

        [Description("Error occurred. Please try after some time.")]
        TransactionError = 999,

        //######################### User Management ##################

        [Description("Role Name already exists. Try another one.")]
        RoleNameAlreadyExists = 101,

        [Description("User Name already exists. Try another one.")]
        UserNameAlreadyExists = 102,

        [Description("Role assigned successfully.")]
        RoleAssignedSuccess = 103,

        [Description("Role based link assigned successfully.")]
        RoleLinkAccessSuccess = 104,

        [Description("User based link assigned successfully.")]
        UserLinkAccessSuccess = 105,

        [Description("Password changed successfully.")]
        PasswordChangeSuccess = 106,

        [Description("Status updated successfully.")]
        UserStatusSuccess = 107,

        [Description("User profile updated successfully.")]
        UserProfileUpdateSuccess = 108,

        [Description("Mobile No. already exists. Please try another one.")]
        MobileNoExists = 109,

        [Description("Email Id already exists. Please try another one.")]
        EmailIDExists = 110,

        [Description("You are not allowed to modify the user as it is inactive.")]
        UserInactive = 111,

        [Description("Invalid email. Please enter your registered email id.")]
        InvalidEmailId = 112,

        [Description("A one time passcode has been sent to your e-mail address valid for 10 minutes.")]
        OTPSuccess = 113,

        [Description("A new password has been sent to your e-mail address.")]
        NewPasswordSuccess = 114,

        [Description("Your OTP has expired. Click on the “Resend OTP” button to generated new one.")]
        OTPExpired = 115,

        [Description("Invalid OTP. Please enter a valid OTP.")]
        InvalidOTP = 116,

        [Description("Role removed successfully from the concerned user.")]
        RoleRemovedSuccess = 117,

        [Description("Sorry, you cann't remove the role from concerned user as links already assigned to user.")]
        RoleRemovedDependency = 118,

        [Description("Invalid Mobile Number. Please enter your registered Mobile Number.")]
        InvalidMobile = 119,

        [Description("A one time passcode has been sent to your registered mobile number valid for 10 minutes.")]
        OTPMobileSuccess = 121,

        [Description("A new password has been sent to your registered mobile number.")]
        NewMobilePassswordSuccess = 122,

        //######################### Soil Testing ##################
        [Description("Soil sample received successfully.")]
        SampleReceiveSuccess = 301,

        [Description("Soil sample already received.")]
        SampleAlreadyReceived = 302,

        [Description("Lab Soil Sample Id already exists. Please try another one.")]
        LabSoilTestIdAlreadyExists = 303,

        [Description("Sample status updated successfully.")]
        SampleStatusUpdated = 304,

        [Description("Soil sample rejected successfully.")]
        SampleRejectSuccess = 305,

        [Description("Test Report added successfully.")]
        TestReportSuccess = 306,

        [Description("Test Report for the concerned test type already added.")]
        TestTypeDuplicate = 307,

        [Description("Test Report deleted successfully.")]
        TestReportDeleted = 308,

        [Description("Test Report approved successfully.")]
        TestReportApproved = 309,

        [Description("Test Report reverted successfully for re-testing.")]
        TestReportReverted = 310,

        [Description("You are not allowed to alter the record as action is being already taken by MOA/ATA.")]
        MOAATAActionTaken = 311,

        [Description("Library assigned successfully to concerned soil sample id(s).")]
        LibraryAssignSuccess = 312,

        [Description("Soil sample id(s) sent successfully to the overseas laboratory.")]
        SenoverseasSuccess = 313,

        [Description("Soil sample already rejected.")]
        SampleAlreadyRejected = 314,

        [Description("Soil sample id(s) already sent to overseas laboratory. Please try another one.")]
        SenoverseasRecordExists = 315,


        [Description("Control Sample added successfully.")]
        ControlSampleSuccess = 316,

        //<<<<<<< .mine
        //        [Description("Control Sample Id already exists. Please try another one.")]
        //        ControlSampleIdAlreadyExists = 317,

        //        [Description("Control Sample Id assigned successfully.")]
        //        ControlSampleIdAssignedSuccess = 318,

        //        [Description("Control Sample Id already assigned. Please try another one.")]
        //        ControlSampleIdAlreadyAssigned = 319,

        //        [Description("Concerned Soil Sample Id already tagged to one Control Sample Id. Please try another one.")]
        //        SoilSampleIdAlreadyTagged = 320,

        //        [Description("Invalid Soil Sample Id. Please try another one.")]
        //        InvalidSoilSampleId = 321,


        //=======
        [Description("Control Sample Id already exists. Please try another one.")]
        ControlSampleIdAlreadyExists = 317,

        [Description("Control Sample Id assigned successfully.")]
        ControlSampleIdAssignedSuccess = 318,

        [Description("Control Sample Id already assigned. Please try another one.")]
        ControlSampleIdAlreadyAssigned = 319,

        [Description("Concerned Soil Sample Id already tagged to one Control Sample Id. Please try another one.")]
        SoilSampleIdAlreadyTagged = 320,

        [Description("Invalid Soil Sample Id. Please try another one.")]
        InvalidSoilSampleId = 321,

        [Description("Sorry, you cann't add more results as the soil sample id already assigned to library.")]
        CanNotAddMoreResult = 322,


        //>>>>>>> .theirs
        //######################### Demography Page ##################
        [Description("Record already existing with same name. Try another one.")]
        DemographyNameExisting = 501,

        [Description("Record already existing with same code. Try another one.")]
        DemographyCodeExisting = 502,
        //######################### Demography Page ##################

        //######################### Survey field page ##################
        [Description("Field already exists. Try another one.")]
        FieldAlreadyExists = 201,

        [Description("Fields that have already been added to forms cannot be edited or deleted.")]
        FieldAlreadyAdded = 202,

        [Description("Forms that have already been assigned to the Surveyor cannot be edited or deleted.")]
        FormAlreadyTagged = 203,

        [Description("Form already exists. Try another one.")]
        FormAlreadyExists = 204,

        [Description("No. of options for the field exceed the allowed value.")]
        MaximumOptionsAllowed = 206,

        [Description("Some Soil Sample ID are already existing. Please check the ones marked in red colour.")]
        SoilSampleIdDup = 205,
        [Description("Error occurred. Please try again after sometime.")]
        ErrorMessage = 6,
        //######################### Survey field page ##################




        //######################### Upload KnimeData page ##################
        [Description("KNIME Result Uploaded successfully!")]

        KnimeResultSuccess = 751,

        [Description("Upload KNIME Result already exist for selected test result!")]

        KnimeResultExists = 752,
        //######################### Upload KnimeData page ##################

        //######################### Upload KnimeData page ##################
        [Description("ML Result Uploaded successfully!")]
        MLResultSuccess = 851,
        [Description("Upload ML Result already exist for selected KNIME Results!")]
        MLResultExists = 852,
        //######################### Upload KnimeData page ##################

        //######################### Schedule KnimeData page ##################
        [Description("KNIME Scheduled successfully!")]

        KnimeScheduleResultSuccess = 753,

        [Description("Upload KNIME Result already exist for selected test result!")]

        KnimeScheduleResultExists = 754,
        //######################### Schedule KnimeData page ##################


        //######################### Sample Request Approve ##################


        [Description("Sample Request Approved.")]
        SampleRequestApproved = 601,

        [Description("Sample Request Rejected.")]
        SampleRequestRejected = 605,


        //######################### Sample Request Approve ##################

        //######################### soil testing Approve ##################

        [Description("Request Approved Successfully.")]
        SoilSampleRequestApproved = 701,

        [Description("Request Rejected Successfully.")]
        SoilSampleRequestRejected = 705,



        //#########################soil testing Approve ##################
        [Description("This Rack number is already assigned to another Region.")]
        rackareadyassined = 88,

        [Description("This drawer is completely filled. Please choose another one.")]
        drawerFilled = 89,

        [Description("This soil sample ID has already been added.")]
        dupSoilSampleId = 90,

        //######################### library archive Approve ##################

        [Description("Request Approved Successfully.")]
        libraryarchiveSampleRequestApproved = 801,

        [Description("Request Rejected Successfully.")]
        libraryarchiveSampleRequestRejected = 805,



        //#########################soil testing Approve ##################

    }
}
