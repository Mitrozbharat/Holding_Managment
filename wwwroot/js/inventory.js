
        $('#selectvendorName').on('click', function () {
            $.ajax({
                url: '/Vendor/GetVendorList', // URL to fetch vendor list
                type: 'GET', // HTTP method
                dataType: 'json', // Expected data type from server
                success: function (response) {

                    $('.dropdownVendor').append('<option value="">Select vendor</option>'); // Add default option
                    $.each(response.data, function (index, vendor) {
                        $('.dropdownVendor').append('<option value="' + vendor.id + '">' + vendor.vendorName + '</option>');
                    });

                },
                error: function (xhr, status, error) {
                    console.error("Error fetching vendor names:", error);
                    alert("An error occurred while fetching vendor data.");
                }
            });
        });


    // When the filter icon is clicked
    $('filter').on('click', function () {
                // Show the modal
                var filterModal = new bootstrap.Modal(document.getElementById('filterModal'));
    filterModal.show();
            });

    $('#applyFilter').on('click', function () {
        $('#filterModal').modal('show');

            });




    function updateFileName() {
                var input = document.getElementById('inventoryimportInput');
             var label = document.getElementById('inventoryimportname');

                // Update the label to show the file name after selecting a file
                if (input.files.length > 0) {
        label.innerHTML = input.files[ 0 ].name;
                }
    }

             
        $(document).ready(function () {

            $('#searchInput').on('input', function () {
                let searchValue = $(this).val();
                if (searchValue.length >= 2) { // Trigger search after 2 characters
                    $.ajax({
                        url: '/search',
                        type: 'GET',
                        data: { name: searchValue },
                        success: function (data) {
                            // Handle the search results here
                            console.log(data); // For example, log the results
                            // Update the UI with the search results
                        },
                        error: function (error) {
                            console.error('Error fetching search results:', error);
                        }
                    });
                }
            });
        });



    document.addEventListener('DOMContentLoaded', function () {
            document.getElementById('getval').addEventListener('change', readURL, true);
        function readURL() {
                        var file = document.getElementById("getval").files[0];
        var reader = new FileReader();
        reader.onloadend = function () {
            document.getElementById('profile-upload').style.backgroundImage = "url(" + reader.result + ")";
                        }
        reader.readAsDataURL(file);
        $('#edit-image').val("");

        if (file) {
                            var reader1 = new FileReader();
        reader1.onloadend = function () {
                                var base64String = reader1.result.replace("data:", "").replace(/^.+,/, ''); // Convert image to Base64 string
        $('#edit-image').val(base64String);
                            };
        reader1.readAsDataURL(file); // Convert the image to Base64 string
                        } else {
            toastr.error('Please select an image file.');
                        }
                   
    }
    document.querySelectorAll('.autocomplete').forEach(function (input) {
        let suggestionsContainer;
    let activeSuggestionIndex = -1;

    input.addEventListener('input', function () {
                        const query = input.value;
    const id = input.getAttribute('data-id');

    let urls = "";
    let inputwidth = "";

    if (id === "vendorName") {
        urls = `/Dashboard/GetVendorName?query=${query}`;
                        }

    if (id === "edit-vendorName") {
        urls = `/Dashboard/GetVendorName?query=${query}`;
                        }
    if (query.length < 1) {
                            if (suggestionsContainer) {
        suggestionsContainer.innerHTML = '';
    suggestionsContainer.style.display = 'none';
                            }
    return;
                        }

    fetch(`${urls}`)
                            .then(response => response.json())
                            .then(data => {
                                if (!suggestionsContainer) {
        suggestionsContainer = document.createElement('div');
    suggestionsContainer.className = 'autocomplete-suggestions';
    input.parentNode.appendChild(suggestionsContainer);
                                }

    if (input.getAttribute('data-width') == "nowidth") {
        suggestionsContainer.style.width = `${input.parentNode.offsetWidth}px`;
                                }


    suggestionsContainer.innerHTML = '';
    suggestionsContainer.style.display = 'block';
    activeSuggestionIndex = -1;

                                data.forEach((item, index) => {
                                    const suggestionItem = document.createElement('div');
    suggestionItem.className = 'autocomplete-suggestion';
    suggestionItem.textContent = item.name;
    suggestionItem.dataset.key = item.id;
    suggestionItem.dataset.value = item.name;

    suggestionItem.addEventListener('click', function () {
        input.value = item.name;
    suggestionsContainer.style.display = 'none';

    if (id === "vendorName") {
        $("#vendorids").val(item.id);
                                        }
    if (id === "edit-vendorName") {
        $("#edit-vendorids").val(item.id);
                                        } 
                                    });
    suggestionsContainer.appendChild(suggestionItem);
                                });
                            });
                    });

    input.addEventListener('keydown', function (e) {

                        const idd = input.getAttribute('data-id');

    const suggestions = suggestionsContainer?.querySelectorAll('.autocomplete-suggestion');
    if (!suggestions) return;

    if (e.key === 'ArrowDown') {
        activeSuggestionIndex = (activeSuggestionIndex + 1) % suggestions.length;
    updateActiveSuggestion(suggestions);
                        } else if (e.key === 'ArrowUp') {
        activeSuggestionIndex = (activeSuggestionIndex - 1 + suggestions.length) % suggestions.length;
    updateActiveSuggestion(suggestions);
                        } else if (e.key === 'Enter') {
        e.preventDefault();
                            if (activeSuggestionIndex > -1) {
                                const activeSuggestion = suggestions[activeSuggestionIndex];
    input.value = activeSuggestion.dataset.value;
    suggestionsContainer.style.display = 'none';


    if (idd === "vendorName") {

        $("#vendorids").val(activeSuggestion.dataset.key);

                                }
    if (idd === "edit-vendorName") {

        $("#edit-vendorids").val(activeSuggestion.dataset.key);

                                } 

                            }
                        }
                    });

    document.addEventListener('click', function (event) {
                        if (suggestionsContainer && !input.contains(event.target)) {
        suggestionsContainer.style.display = 'none';
                        }
                    });

    function updateActiveSuggestion(suggestions) {
        suggestions.forEach((suggestion, index) => {
            suggestion.classList.toggle('active', index === activeSuggestionIndex);
        });
                    }
                });
            });

    function uploadFile() {
                    var fileInput = document.getElementById('inventoryimportInput');
    var file = fileInput.files[0];

    if (file) {
                  
                        var formData = new FormData();
    formData.append('file', file);

    fetch('/Dashboard/Upload',
    {
        method: 'POST',
    body: formData,
                        })
                        .then(response => {
                            if (!response.ok) {
                                throw new Error('Network response was not ok');
                            }
    return response.text(); // Get response as text first
                        })
                        .then(text => {
                            try {
                                const data = JSON.parse(text); // Try to parse text as JSON
    console.log('Success:', data);
    location.reload();
                            } catch (e) {
        console.error('Failed to parse JSON:', text); // Handle parsing error
                            }
                        })
                        .catch(error => {
        console.error('Error:', error);
                        });
                    } else {
        alert('Please select a file to upload.'); 
                    }
                }


        function handleFile(e) {
            var file = e.target.files[ 0 ];
            var reader = new FileReader();

            reader.onload = function (event) {
                var data = new Uint8Array(event.target.result);
                var workbook = XLSX.read(data, { type: 'array' });
                var firstSheetName = workbook.SheetNames[ 0 ];
                var worksheet = workbook.Sheets[ firstSheetName ];

                // Convert the Excel sheet to JSON array
                var jsonData = XLSX.utils.sheet_to_json(worksheet, { header: 1 });

                // Initialize Handsontable with the JSON data
                initHandsontable(jsonData);
            };

            reader.readAsArrayBuffer(file);
        }

        function initHandsontable(data) {
            var container = document.getElementById('handsontable-preview');

    // Clear any previous Handsontable instance
    if (container.handsontableInstance) {
        container.handsontableInstance.destroy();
            }

    // Initialize a new Handsontable instance
    container.handsontableInstance = new Handsontable(container, {
        data: data,
    rowHeaders: true,
    colHeaders: true,
    stretchH: 'all',
    height: 400,
    licenseKey: 'non-commercial-and-evaluation' // For non-commercial use only
            });
        }

    function Openlivepreview(imageSrc) {
            // Get the target image element where you want to apply the new src
            var targetImage = document.getElementById('targetImage');

    // If the imageSrc contains a relative URL (like '~/images/Shahu-logo.png'), convert it to an absolute path
    if (imageSrc.startsWith('~/')) {
        // Assuming the app runs at the root of the site, you can replace '~/' with the actual root path
        imageSrc = imageSrc.replace('~/', window.location.origin + '/');
            }

    // Set the source of the target image to the clicked image's src
    targetImage.src = imageSrc;

    // If you want to display the image in a modal or popup, you can add that logic here
    $('#imagePreviewModal').modal('show'); // Assuming you are using Bootstrap modal
        }


    // Set the values of the form fields in the modal
    function editCratModal(id, city, area, width, height, rate, VendorName, Image, vendorid, location, vendoramt, Type) {

      
         $('#edit-image').val("");
        $('#id').val(id);
        $('#edit-city').val(city);
        $('#edit-Area').val(area);
        $('#edit-location').val(location);
        $('#edit-width').val(width);
        $('#edit-height').val(height);
        $('#edit-rate').val(rate);
        $('#edit-vendoramt').val(vendoramt);
        $('#edit-vendorName').val(VendorName);
        $('#edit-vendorids').val(vendorid);
        $('#styped').val(Type);


        if (Image)
        {
                        const base64Image = 'data:image/png;base64,' + Image;
        // Select the div element by its ID
        const divElement = document.getElementById('profile-upload');

        // Set the background image using the base64 data
        divElement.style.backgroundImage = `url(${base64Image})`;
        //  $('#modalImage').attr('src', 'data:image/jpeg;base64,' + Image);
        $('#edit-image').val(Image);
                } 
            }
    $('#editInventory').click(function ()
    {
        var id = $('#id').val();
        var city = $('#edit-city').val();
        var area = $('#edit-Area').val();
        var width = $('#edit-width').val();
        var height = $('#edit-height').val();
        var rate = $('#edit-rate').val();
        var vendorName = $('#edit-vendorName').val();
        var vendorid = $('#edit-vendorids').val();
        var imageFile = $('#edit-image').val(); // Get the selected image file
        var locations = $('#edit-location').val(); // Get the selected image file
        var vendoramt = $('#edit-vendoramt').val(); // Get the selected image file
        var st = $('#styped').val(); // Get the selected image file

    if (imageFile !="" && vendorName != "")
    {

        // Send the form data along with the Base64 string via AJAX
        $.ajax({
            type: "POST",
            url: '/Dashboard/UpdateInventoryItems',// Update the URL to match your controller action
            data: {
                Id: id,
                City: city,
                Area: area,
                Width: width,
                Height: height,
                Rate: rate,
                VendorName: vendorName,
                vendorid: vendorid,
                Image: imageFile,
                location: locations,
                vendoramt: vendoramt,
                st: st,
            },
            success: function (response) {
                if (response.success) {
                    toastr.success(response.message);
                    $('#editModal').modal('hide'); // Close the modal
                    location.reload();
                } else {
                    toastr.error(response.message);
                }
            },
            error: function () {
                toastr.error('An error occurred while updating the item.');
            }
        });
                   
                }
    else
    {
                    if (imageFile) {
        toastr.error('Please select vendor name.');
                    }
    else {
        toastr.error('Please select an image file.');
                    }
                    
                }
            });
            


        function setDeleteItem(Id) {
            $('#selectedId').val(Id);

            $('#cnfdelete').off('click').on('click', function () {
                var selectedInventoryId = $('#selectedId').val();
                console.log("Selected ID: " + selectedInventoryId);

                $.ajax({
                    url: '/Dashboard/DeleteInventory/' + selectedInventoryId, // Pass ID in the URL
                    type: 'DELETE',
                    success: function (response) {
                        console.log("Deleted: " + response);

                        if (response.success) {
                            toastr.success(response.message);
                            location.reload();
                        } else {
                            toastr.error(response.message);
                        }
                    },
                    error: function (xhr, status, error) {
                        console.error('Error deleting inventory:', error);
                        toastr.error('Error deleting inventory.');
                    }
                });
            });
        }

            function createItem(id, image, area, city, width,height, rate, fkVendorId)
    {
                if(fkVendorId != 0 && fkVendorId != null){
        // Assign values to the form fields
        $('#id').val(id);
    $('#image').val(image);
    $('#area').val(area);
    $('#city').val(city);
    $('#width').val(width);
    $('#height').val(height);
    $('#rate').val(rate);
    $('#fk_VendorId').val(fkVendorId);
    // $('#bookingStatus').val(bookingStatus);

    $.ajax({
        type: "POST",
    url: '/Dashboard/CreateInventoryItems', // Ensure the URL matches the action method
    data: {
        Id: id,
    Image: image,
    Area: area,
    City: city,
    Width: width,
    Height: height,
    Rate: rate,
    FkVendorId: fkVendorId,
                        },
    success: function (response) {
                            if (response.success) {
        toastr.success(response.Message); // Display the success message from the response
    location.reload(); // Optionally reload the page
                            } else {
        toastr.error(response.Message);
    alert("Already Added!");// Display the error message from the response
                            }
                        },
    error: function (xhr, status, error) {
        toastr.error('An error occurred while adding the quotation.');
                        }
                    });
                }
    else{
        toastr.error("This inventory does not have a vendor please add vendor");
                }
            }

    const imageInput = document.getElementById('imageInput');
    const fileName = document.getElementById('fileName');

    $('imageInput').on('change', function () {
                const file = this.files[0];

    if (file)
    {
        fileName.textContent = file.name;
                } else
    {
        fileName.textContent = 'No file chosen';
                }
            });


    const imageInput1 = document.getElementById('imageInputt');
    const fileName1 = document.getElementById('fileNamee');

    $('imageInput1').on('change', function ()
    {
                const file = this.files[0];

    if (file)
    {
        fileName1.textContent = file.name;
                } else
    {
        fileName1.textContent = 'No file chosen';
                }
            });





        function mainfiltericon() {
            // document.getElementById('mybtn').style.display = 'none';

            $('#filterModal').modal('show');

        }
            function updateRangeValue(value) {
        // Update the range value displayed next to the slider
        //document.getElementById('rangeValue').innerText = (value / 100000).toFixed(2) + ' Lakh';
        document.getElementById('rangeValue').innerText = "0 To " + value + "/-";

    $("#filterrangeValue").val("");
    $("#filterrangeValue").val(""+value+"");

    // Here you can fetch the range value to filter data


    console.log("Selected Range: " + value);

                // fetchFilteredData(value);
            }

    function searchInventory() {
               // var searchQuery = document.getElementById('searchQuery').value;
                var searchQuery ="";
    var pageSize = document.getElementById('rowQuantity').value;
    var amount = '@(string.IsNullOrEmpty(Model.amount) ? "" : Model.amount)';
    var vendor = '@(string.IsNullOrEmpty(Model.vendor) ? "0" : Model.vendor)';
    var City = '@(string.IsNullOrEmpty(Model.City) ? "" : Model.City)';
    var Area = '@(string.IsNullOrEmpty(Model.Area) ? "" : Model.Area)';
    var Width = '@(string.IsNullOrEmpty(Model.Width) ? "" : Model.Width)';
    var Height = '@(string.IsNullOrEmpty(Model.Height) ? "" : Model.Height)';
    window.location.href = `/Dashboard/HoardingInventory?searchQuery=${searchQuery}&amount=${amount}&vendor=${vendor}&City=${City}&Area=${Area}&Width=${Width}&Height=${Height}&pageNumber=1&pageSize=${pageSize}`;
            }

    function updateRowQuantity() {
                var pageSize = document.getElementById('rowQuantity').value;
    //  var searchQuery = document.getElementById('searchQuery').value;
    var searchQuery = "";
    var amount = '@(string.IsNullOrEmpty(Model.amount) ? "" : Model.amount)';
    var vendor = '@(string.IsNullOrEmpty(Model.vendor) ? "0" : Model.vendor)';
    var City = '@(string.IsNullOrEmpty(Model.City) ? "" : Model.City)';
    var Area = '@(string.IsNullOrEmpty(Model.Area) ? "" : Model.Area)';
    var Width = '@(string.IsNullOrEmpty(Model.Width) ? "" : Model.Width)';
    var Height = '@(string.IsNullOrEmpty(Model.Height) ? "" : Model.Height)';
    window.location.href = `/Dashboard/HoardingInventory?searchQuery=${searchQuery}&amount=${amount}&vendor=${vendor}&City=${City}&Area=${Area}&Width=${Width}&Height=${Height}&pageNumber=1&pageSize=${pageSize}`;
            }


    function Applayfilter(){
                var pageSize = document.getElementById('rowQuantity').value;
    var searchQuery = "";
    //  var searchQuery = document.getElementById('searchQuery').value;
    //cityFilter areaFilter filterwidth filterheight vendoramtFilter
    var amount=$("#filterrangeValue").val();
    var vendor = $("#vendoramtFilter").val();
    var City=$("#cityFilter").val();
    var Area=$("#areaFilter").val();
    var Width=$("#filterwidth").val();
    var Height=$("#filterheight").val();
    window.location.href = `/Dashboard/HoardingInventory?searchQuery=${searchQuery}&amount=${amount}&vendor=${vendor}&City=${City}&Area=${Area}&Width=${Width}&Height=${Height}&pageNumber=1&pageSize=${pageSize}`;

            }

    function removefilter(){


                var pageSize = document.getElementById('rowQuantity').value;
    //  var searchQuery = document.getElementById('searchQuery').value;
    var searchQuery = "";
    //cityFilter areaFilter filterwidth filterheight vendoramtFilter
    var amount="";
    var vendor = "0";
    var City="";
    var Area="";
    var Width="";
    var Height="";
    window.location.href = `/Dashboard/HoardingInventory?searchQuery=${searchQuery}&amount=${amount}&vendor=${vendor}&City=${City}&Area=${Area}&Width=${Width}&Height=${Height}&pageNumber=1&pageSize=${pageSize}`;

            }



    document.querySelectorAll('.autocomplete').forEach(function (input)
    {
        let suggestionsContainer;
    let activeSuggestionIndex = -1;
    let debounceTimer;

    input.addEventListener('input', function () {
        clearTimeout(debounceTimer);
                debounceTimer = setTimeout(() => {
                    const query = input.value.trim();
    const id = input.getAttribute('data-id');
    let urls = "";

    if (id === "vendorName" || id === "edit-vendorName") {
        urls = `/Dashboard/GetVendorName?query=${query}`;
                }

    if (query.length < 1) {
                    if (suggestionsContainer) {
        suggestionsContainer.innerHTML = '';
    suggestionsContainer.style.display = 'none';
                    }
    return;
                }

    // Fetch suggestions from the server
    fetch(urls)
                    .then(response => response.json())
                    .then(data => {
                        if (!suggestionsContainer) {
        suggestionsContainer = document.createElement('div');
    suggestionsContainer.className = 'autocomplete-suggestions';
    input.parentNode.appendChild(suggestionsContainer);
                        }

    suggestionsContainer.innerHTML = '';
    suggestionsContainer.style.display = 'block';
    activeSuggestionIndex = -1;

    if (data.length === 0) {
        // Show "No matches found" if no data
        suggestionsContainer.innerHTML = `<div class="no-results">No matches found</div>`;
    return;
                        }

                        // Populate suggestions
                        data.forEach((item) => {
                            const suggestionItem = document.createElement('div');
    suggestionItem.className = 'autocomplete-suggestion';
    suggestionItem.textContent = item.name;
    suggestionItem.dataset.key = item.id;
    suggestionItem.dataset.value = item.name;

    // Handle suggestion click
    suggestionItem.addEventListener('click', function () {
        selectSuggestion(item, input, suggestionsContainer);
                            });
    suggestionsContainer.appendChild(suggestionItem);
                        });
                    });
              }, 300); // Debounce delay
          });

    input.addEventListener('keydown', function (e) {
                            const suggestions = suggestionsContainer?.querySelectorAll('.autocomplete-suggestion');
    if (!suggestions || suggestions.length === 0) return;

    if (e.key === 'ArrowDown') {
        activeSuggestionIndex = (activeSuggestionIndex + 1) % suggestions.length;
    updateActiveSuggestion(suggestions);
                            } else if (e.key === 'ArrowUp') {
        activeSuggestionIndex = (activeSuggestionIndex - 1 + suggestions.length) % suggestions.length;
    updateActiveSuggestion(suggestions);
                            } else if (e.key === 'Enter') {
        e.preventDefault();
                                if (activeSuggestionIndex > -1) {
                                    // Select the active suggestion on "Enter"
                                    const activeSuggestion = suggestions[activeSuggestionIndex];
    input.value = activeSuggestion.dataset.value;
    suggestionsContainer.style.display = 'none';

    if (input.getAttribute('data-id') === "vendorName") {
        $("#vendorids").val(activeSuggestion.dataset.key);
                                    }
    if (input.getAttribute('data-id') === "edit-vendorName") {
        $("#edit-vendorids").val(activeSuggestion.dataset.key);
                                    }
                                } else {
        // If no suggestion is highlighted, match the entered text
        matchAndSelectSuggestion(query, data, input, suggestionsContainer);
                                }
                            }
                        });

    document.addEventListener('click', function (event) {
                            if (suggestionsContainer && !input.contains(event.target)) {
        suggestionsContainer.style.display = 'none';
                            }
                        });

    function updateActiveSuggestion(suggestions) {
        suggestions.forEach((suggestion, index) => {
            suggestion.classList.toggle('active', index === activeSuggestionIndex);
        });
                        }

    function selectSuggestion(item, input, suggestionsContainer) {
        input.value = item.name;
    suggestionsContainer.style.display = 'none';

    if (input.getAttribute('data-id') === "vendorName") {
        $("#vendorids").val(item.id);
                            }
    if (input.getAttribute('data-id') === "edit-vendorName") {
        $("#edit-vendorids").val(item.id);
                            }
                        }

    function matchAndSelectSuggestion(query, data, input, suggestionsContainer) {
                            const matchedItem = data.find(item => item.name.toLowerCase() === query.toLowerCase());
    if (matchedItem) {
        selectSuggestion(matchedItem, input, suggestionsContainer);
                            } else {
        suggestionsContainer.innerHTML = `<div class="no-results">No exact match found</div>`;
                            }
                        }
                    });




