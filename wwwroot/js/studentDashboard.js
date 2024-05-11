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
        UploadViewer.style.transition = "bottom 1s";
        UploadViewer.style.bottom = "0";
    } else {
        UploadViewer.style.transition = "bottom 1s";
        UploadViewer.style.bottom = "-100%";
    }
   
}