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
                `;
                nestUl.appendChild(li);
            });
            ul.appendChild(li);
            li.appendChild(nestUl);
        });    
    
    
};
const GetStudentResults = (event) => {
    console.log('GetStudentResults was called');
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