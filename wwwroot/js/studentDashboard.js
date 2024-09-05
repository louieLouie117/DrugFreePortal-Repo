const GetUserFilesHandler = async () => {
    console.log("GetUserFilesHandler called");
    // clear this element with id StudentResults
    document.getElementById('StudentResults').innerHTML = "";
    fetch("/getStudentFiles")
    .then(response => response.json())
    .then(data => {
        console.log("data from db", data);
        console.log("student file list", data.studentFiles);

        RenderStudentFiles(data.studentFiles);
    });
};

// arrow function to render student files for ul with id StudentFiles
const RenderStudentFiles = (files) => {
    const ul = document.getElementById('StudentFiles'); // Assuming you have a ul with id 'StudentFiles'
    // Clear the StudentFiles id ul
    ul.innerHTML = "";

    files.forEach(file => {
        // Create a new list item
        console.log("file", file.fileName);
       

        const li = document.createElement('li');
        // add to the bottom if needed <img src="${file.filePath}" alt="${file.filePath}">

        li.innerHTML = `
            <a href="${file.filePath}" target="_blank">${file.fileName}</a>
        `;
    
        // Append the list item to the ul
        ul.appendChild(li);
    });
};


const UploadFileViewHandler = async (e) => { 
    let UploadViewer = document.getElementById("UploadFilesContainer");

    if(e.target.innerHTML === "Upload Documents") {
        UploadViewer.style.transition = "bottom 0.5s ease-in-out";
        UploadViewer.style.bottom = "0";
        GetStudentSchoolComplianceHandler();
    } else {
        UploadViewer.style.transition = "bottom 0.5s ease-in-out";
        UploadViewer.style.bottom = "-300%";
    }
   
}

// Function to upload a file
const GetComplianceFilesHandler = async (id) => {
    console.log("GetComplianceFilesHandler called", id);

    fetch('getComplianceFiles', {
        method: "GET",

    })
    .then(response => response.json())
    .then(data => {
        console.log("data from db", data);
        console.log("student file list", data.studentFiles);
    })
    .catch(error => {
        console.error("Error fetching compliance files:", error);
        // Handle the error here
    });
}



const uploadFile = (id, name) => {
    let button = document.getElementById("uploadButton_" + id);
    let form = document.getElementById("uploadForm_" + id);

    // Check if the event listener has already been added
    if (!form.dataset.listenerAdded) {
        form.addEventListener("submit", function (event) {
            event.preventDefault();
            console.log(id, name);

            var formData = new FormData();
            var fileInput = document.getElementById("fileInput_" + id);
            var file = fileInput.files[0];
            console.log("------------file", file);

            if (!file) {
                alert("No file selected");
                return;
            }

            // Change the file name to "file" if you want to use the same name
            formData.append("file", file, name);
            console.log("here", file);

            var xhr = new XMLHttpRequest();
            xhr.open("POST", "/UploadSingleFile", true);
            xhr.onload = function () {
                if (xhr.status === 200) {
                    // alert("File uploaded successfully!");
                    // Call any other necessary functions here
                    button.disabled = false;
                    button.innerHTML = "Uploading...";
                    button.style.backgroundColor = "gray";

                    const label = document.getElementById("cardLabel_" + id);
           
                    setTimeout(() => {
                        label.innerHTML = "Uploaded complete. You can now upload other files for " + name + " compliance.";
                        label.style.color = "green";
                        document.getElementById("uploadForm_" + id).reset();
                        button.style.backgroundColor = "#245684";
                        button.innerHTML = "Upload";

                    }, 1500);

                    // Call the function to get the updated list of files
                    GetComplianceFilesHandler(id, name);
                
                } else {
                    alert("An HTTP error occurred. Please try again.");
                }
            };
            xhr.onerror = function () {
                alert("A network error occurred. Please try again.");
                return;
            };
            xhr.send(formData);
        });

        // Mark that the event listener has been added
        form.dataset.listenerAdded = "true";
    }
};

const RenderStudentCompliance = (complianceList) => {
    const ul = document.getElementById('SchoolComplianceForStudent'); // Assuming you have a ul with id 'SchoolComplianceForStudent'
    // Clear the SchoolComplianceForStudent id ul
    ul.innerHTML = "";

    complianceList.forEach(compliance => {
        // Create a new list item
        console.log("compliance", compliance.complianceTypeId);

        const li = document.createElement('li');
        li.id = "UploadCard_" + compliance.complianceTypeId;
        li.innerHTML = `
            <label id="cardLabel_${compliance.complianceTypeId}">${compliance.name}</label>
            <form class="FileUploadContainer" id="uploadForm_${compliance.complianceTypeId}">
            <input type="file" id="fileInput_${compliance.complianceTypeId}" name="file" />
            <footer>
            <button id="uploadButton_${compliance.complianceTypeId}" class="mainBTN" type="submit">Upload</button>
            </footer>
            </form>
            <ul id="FileUploadList_${compliance.complianceTypeId}"></ul>
        `;

      

        // Append the list item to the ul
        ul.appendChild(li);

        // Call uploadFile to set up the event listener
        uploadFile(compliance.complianceTypeId, compliance.name);
    });
};

const GetStudentSchoolComplianceHandler = async () => {
    console.log("GetStudentSchoolComplianceHandler called");
    fetch("/GetStudentSchoolCompliance")
    .then(response => response.json())
    .then(data => {
        console.log("data from db", data);
        console.log("student compliance", data.complianceListData);

        RenderStudentCompliance(data.complianceListData);
    })
    .catch(error => {
        console.error("Error fetching student compliance:", error);
        // Handle the error here
    });
}


const RenderStudentResults = (data) => {
    console.log('Render Student Results was called', data);
    const ul = document.getElementById('StudentResults'); // Assuming you have a ul with id 'StudentResults'
   // foreach loop to iterate through the data and create a label with for each semester
       ul.innerHTML = "";
        data.forEach(result => {
            console.log("result", result);
            const li = document.createElement('li');
            li.innerHTML = `
                <label>${result.semester.title}</label>
            `;

            const nestUl = document.createElement('ul');
            // loop through the results for each records and create a label for each
            result.records.forEach(record => {
                const li = document.createElement('li');
                li.innerHTML = `
                    <label>${record.complianceType}</label>
                    <label style="background-color:#${record.statusColor}; color: white;">${record.complianceStatus}</label>
                `;
                nestUl.appendChild(li);
            });
            ul.appendChild(li);
            li.appendChild(nestUl);
        });    
    
    
};
const GetStudentResults = (event) => {
    console.log('GetStudentResults was called');
    // clear this element with id StudentFiles
    document.getElementById('StudentFiles').innerHTML = "";
    fetch('/studentResults')
        .then(response => response.json())
        .then(data => {
            console.log("Data is here", data.data);
            RenderStudentResults(data.data);
        })

        .catch(error => {
            console.error('Error:', error);
        });
};