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
        li.innerHTML = `
            <img src="${file.filePath}" alt="${file.filePath}">
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




const uploadFile = (name) => {
    document.getElementById("uploadForm_"+ name).addEventListener("submit", function (event) {
        event.preventDefault();

        var formData = new FormData();
        var fileInput = document.getElementById("fileInput_" + name);
        var file = fileInput.files[0];

        if (file) {
            formData.append("file", file);
            console.log(file);

            var xhr = new XMLHttpRequest();
            xhr.open("POST", "/UploadSingleFile", true);
            xhr.onload = function () {
                if (xhr.status === 200) {
                    alert("File uploaded successfully!");
                    // Call any other necessary functions here
                } else {
                    alert("An error occurred.");
                }
            };
            xhr.onerror = function () {
                alert("An error occurred.");
            };
            xhr.send(formData);
        } else {
            alert("No file selected");
        }
    });
};



const RenderStudentCompliance = (complianceList) => {
    const ul = document.getElementById('SchoolComplianceForStudent'); // Assuming you have a ul with id 'SchoolComplianceForStudent'
    // Clear the SchoolComplianceForStudent id ul
    ul.innerHTML = "";

    complianceList.forEach(compliance => {
        // Create a new list item
        console.log("compliance", compliance.name);

        const li = document.createElement('li');
        li.innerHTML = `

            <form class="FileUploadContainer" id="uploadForm_${compliance.name}">


            <input type="file" id="fileInput_${compliance.name}" name="file" />
              <footer>
                <label>${compliance.name}</label>

               <button id="uploadButton_${compliance.name}" class="mainBTN" onclick="uploadFile('${compliance.name}')">Upload</button>
               </footer>
        
            </form>
             
        `;

        // Append the list item to the ul
        ul.appendChild(li);
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