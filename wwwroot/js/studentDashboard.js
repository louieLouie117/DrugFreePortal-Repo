const GetUserFilesHandler = async () => {
    console.log("GetUserFilesHandler called");
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
            <img src="/img/uploads/${file.fileName}" alt="${file.fileName}">
            <button onclick="window.open('/img/uploads/${file.fileName}', '_blank')">View</button>
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

const RenderStudentCompliance = (complianceList) => {
    const ul = document.getElementById('SchoolComplianceForStudent'); // Assuming you have a ul with id 'SchoolComplianceForStudent'
    // Clear the SchoolComplianceForStudent id ul
    ul.innerHTML = "";

    complianceList.forEach(compliance => {
        // Create a new list item
        console.log("compliance", compliance.name);

        const li = document.createElement('li');
        li.innerHTML = `
            <label>${compliance.name}</;>
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


const RenderSudentResults = (data) => {
    console.log('RenderSudentResults was called');
    const StudentResults = document.getElementById('StudentResults');
    StudentResults.innerHTML = '';
    data.forEach((item) => {
        console.log('item', item);
        const listItem = document.createElement('li');

        const complianceType = document.createElement('label');
        complianceType.innerHTML = item.complianceType;
        listItem.appendChild(complianceType);

        const complianceStatus = document.createElement('label');
        complianceStatus.innerHTML = item.complianceStatus;
        complianceStatus.style.backgroundColor = "#" + item.statusColor;
        complianceStatus.style.color = 'white';
        listItem.appendChild(complianceStatus);


        StudentResults.appendChild(listItem);
    });
};
const GetStudentResults = (event) => {
    console.log('GetStudentResults was called');
    fetch('/studentResults')
        .then(response => response.json())
        .then(data => {
            console.log("Data is here", data.message, data, data.data);
            // RenderSudentResults(data.data);
        })

        .catch(error => {
            console.error('Error:', error);
        });
};