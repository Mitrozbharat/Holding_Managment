//$(document).ready(function () {

//    $('#saveVendorButton').click(function () {
//        event.preventDefault(); // Prevent the default form submission
//        // Clear previous error messages
//        $('.error-message').remove();

//        // Validate required fields     businessName VendorPersonName email gstn contactNumber
//        var isValid = true;
//        const hasNumber = /\d/;
//        const textRegex = /^[a-zA-Z\s]+$/; // Text only (letters and spaces)
//        const contactNumberRegex = /^\d{10}$/; // Exactly 10 digits


//        let businessName = document.getElementsByClassName('businessName')[ 0 ].value.trim();

//        let VendorPersonName = document.getElementsByClassName('VendorPersonName')[ 0 ].value.trim();

//        let email = document.getElementsByClassName('email')[ 0 ].value.trim();

//        let gstn = document.getElementsByClassName('gstn')[ 0 ].value.trim();

//        let contactNumber = document.getElementsByClassName('contactNumber')[ 0 ].value.trim();

//        let alternateNumber = document.getElementsByClassName('alternateNumber')[ 0 ].value.trim();

//        let city = document.getElementsByClassName('city')[ 0 ].value.trim();

//        let address = document.getElementsByClassName('address')[ 0 ].value.trim();

//        let state = document.getElementsByClassName('state')[ 0 ].value.trim();



//        // Check Business Name
//        if ($('#businessName').val().trim() === '') {
//            $('#businessName').after('<span class="error-message text-danger">This field is required.</span>');
//            isValid = false;

//        } else if (!textRegex.test(businessName)) {
//            $('.businessName').after('<span class="error-message text-danger">Business Name must contain only letters.</span>');
//            isValid = false;
//        }

//        // Check Contact Person Name
//        if ($('#VendorPersonName').val().trim() === '') {
//            $('#VendorPersonName').after('<span class="error-message text-danger">This field is required.</span>');
//            isValid = false;
//        } else if (!textRegex.test(VendorPersonName)) {
//            $('.VendorPersonName').after('<span class="error-message text-danger">Vendor Name must contain only letters.</span>');
//            isValid = false;
//        }


//        if ($('.email').val().trim() === '') {
//            $('.email').after('<span class="error-message text-danger">This field is required.</span>');
//            isValid = false;
//        } else if (!validateEmail(email)) {
//            $('.email').after('<span class="error-message text-danger">Enter a valid email address.</span>');
//            isValid = false;
//        }

//        function validateEmail(email) {
//            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
//            return emailRegex.test(email);
//        }
//        // Check Contact Numbe

//        if (contactNumber === '') {
//            $('.contactNumber').after('<span class="error-message text-danger">This field is required.</span>');
//            isValid = false;
//        }
//        else if (!contactNumberRegex.test(contactNumber)) {
//            $('.contactNumber').after('<span class="error-message text-danger">Contact Number must be exactly 10 digits.</span>');
//            isValid = false;
//        }



//        // Check if GST Number (if filled) is exactly 15 characters long
//        var gstNumber = $('#gstn').val().trim();
//        if (gstNumber !== '' && gstNumber.length !== 15) {
//            $('#gstn').after('<span class="error-message text-danger">GST Number must be exactly 15 characters.</span>');
//            isValid = false;
//        }


//        if ($('#city').val().trim() === '') {
//            $('#city').after('<span class="error-message text-danger">This field is required.</span>');
//            isValid = false;
//        } else if (!textRegex.test(city)) {
//            $('.city').after('<span class="error-message text-danger">City  must contain only letters.</span>');
//            isValid = false;
//        }



//        if (address === '') {
//            $('.address').after('<span class="error-message text-danger">This field is required.</span>');
//            isValid = false;
//        }
//        else if (!textRegex.test(address)) {
//            $('.address').after('<span class="error-message text-danger"> must contain only letters and spaces.</span>');
//            isValid = false;
//        }

//        if (state === '') {
//            $('.state').after('<span class="error-message text-danger">This field is required.</span>');
//            isValid = false;
//        }




//        // If the form is valid, proceed with the AJAX submission
//        if (isValid) {
//            var formData = $('#addVendorForm').serialize();
//            console.log(formData); // Check serialized data in console

//            $.ajax({
//                type: "POST",
//                url: '/Vendor/AddVendor', // Match the controller action name
//                data: formData,
//                success: function (response) {
//                    console.log("success" + response);
//                    if (response.message == "Vendor Add Successfully. ") {
//                        toastr.success('Vendor added successfully.');
//                        location.reload(); // Optionally reload the page
//                    }
//                    else {
//                        toastr.error('An error occurred while adding the customer.');
//                    }

//                },
//                error: function (xhr, status, error) {
//                    console.error(xhr.responseText); // Log any errors
//                    toastr.error('An error occurred while adding the vendor.');
//                }
//            });
//        } else {
//            toastr.error('Please fill all the required fields.');
//        }


//    });
//});


//$('#updateVendorButton').on('click', function () {

//    $('.error-message').remove();
//    alert("ok");


//    const hasNumber = /\d/;
//    const textRegex = /^[a-zA-Z\s]+$/; // Text only (letters and spaces)
//    const contactNumberRegex = /^\d{10}$/; // Exactly 10 digits

//    var isValid = true;
//    var vendorId = $('#editVendorId').val(); // Get vendor ID from hidden input
//    let editBusinessName = document.getElementsByClassName('editBusinessName')[ 0 ].value.trim();
//    let editVendorPersonName = document.getElementsByClassName('editVendorPersonName')[ 0 ].value.trim();
//    let editContactNo = document.getElementsByClassName('editContactNo')[ 0 ].value.trim();
//    let editemail = document.getElementsByClassName('editemail')[ 0 ].value.trim();
//    let editGstNo = document.getElementsByClassName('editGstNo')[ 0 ].value.trim();
//    let editalternateNumber = document.getElementsByClassName('alternateNumber')[ 0 ].value.trim();
//    let editcity = document.getElementsByClassName('editcity')[ 0 ].value.trim();
//    let editaddress = document.getElementsByClassName('editaddress')[ 0 ].value.trim();
//    let editstate = document.getElementsByClassName('editstate')[ 0 ].value.trim();


//    // Check Business Name
//    if ($('#editBusinessName').val().trim() === '') {
//        $('#editBusinessName').after('<span class="error-message text-danger">This field is required.</span>');
//        isValid = false;
//    } else if (!textRegex.test(editBusinessName)) {
//        $('.editBusinessName').after('<span class="error-message text-danger">Business Name must contain only letters.</span>');
//        isValid = false;
//    }

//    // Check Contact Person Name
//    if ($('#editVendorPersonName').val().trim() === '') {
//        $('#editVendorPersonName').after('<span class="error-message text-danger">This field is required.</span>');
//        isValid = false;
//    } else if (!textRegex.test(editVendorPersonName)) {
//        $('.editVendorPersonName').after('<span class="error-message text-danger">Vendor Name must contain only letters.</span>');
//        isValid = false;
//    }


//    // Check Email
//    if ($('#editemail').val().trim() === '') {
//        $('#editemail').after('<span class="error-message text-danger">This field is required.</span>');
//        isValid = false;
//    } if (!validateEmail(editemail)) {
//        $('.editemail').after('<span class="error-message text-danger">Enter a valid email address.</span>');
//        isValid = false;
//    }

//    function validateEmail(email) {
//        // Simple email validation regex
//        const emailRegex = /^[^\s@@]+@@[^\s@@]+\.[^\s@@]+$/;
//        return emailRegex.test(email);
//    }


//    // Check Contact Number
//    if ($('#editContactNo').val().trim() === '') {
//        $('#editContactNo').after('<span class="error-message text-danger">This field is required.</span>');
//        isValid = false;
//    }

//    if (editContactNo === '') {
//        $('.editContactNo').after('<span class="error-message text-danger">This field is required.</span>');
//        isValid = false;
//    }
//    else if (!contactNumberRegex.test(editContactNo)) {
//        $('.editContactNo').after('<span class="error-message text-danger">Contact Number must be exactly 10 digits.</span>');
//        isValid = false;
//    }

//    // Check if GST Number (if filled) is exactly 15 characters long
//    var gstNumber = $('#editGstNo').val().trim();
//    if (gstNumber !== '' && gstNumber.length !== 15) {
//        $('#editGstNo').after('<span class="error-message text-danger">GST Number must be exactly 15 characters.</span>');
//        isValid = false;
//    }

//    // If the form is valid, proceed with the AJAX submission
//    if (isValid) {

//        var formData = {
//            Id: vendorId,
//            BusinessName: editBusinessName,
//            VendorName: editVendorPersonName,
//            City: editcity,
//            Address: editaddress,
//            Email: editemail,
//            AlternateNumber: editalternateNumber,
//            ContactNo: editContactNo,
//            State: editstates

//        };

//        console.log("Updating Vendor with ID: " + formData.Id);
        
//        $.ajax({
//            type: "PUT",
//            url: '/Vendor/EditVendor', // Update URL as needed
//            data: JSON.stringify(formData),
//            success: function (response) {
//                if (response.success) {
//                    toastr.success('Vendor updated successfully.');
//                    $('#editVendorModal').model('hide');
//                    location.reload(); // Reload the page after successful update
//                } else {
//                    toastr.error('Error updating vendor.');
//                }
//            },
//            error: function (xhr, status, error) {
//                console.error(xhr.responseText); // Log any errors
//                toastr.error('An error occurred while updating the vendor.');
//            }
//        });

//    } else {
//        toastr.error('Please fill all the required fields.');
//    }


//});


