console.log("Evaluator Dashboard Loaded");

// render on load
document.addEventListener('DOMContentLoaded', function () {
    fetchCurrentSemesters();
    
});

// arrow function to fetch current semesters from the database
const fetchCurrentSemesters = async () => {
    console.log("***fetchCurrentSemesters");
    try {
        const response = await fetch("/GetCurrentSemesters", {
            method: "GET"
        });
        const data = await response.json();
        console.log("data from db", data);
        console.log("Current Semester", data.semesterData);
        console.log("Current Semester", data.semesterData[0].semesterId);
        console.log("Current Semester", data.semesterData[0].title);

        document.getElementById('SemesterId').innerHTML = data.semesterData[0].semesterId;
        document.getElementById('SemesterTitle').innerHTML = data.semesterData[0].title;

        

       
    } catch (error) {
        console.error("Error fetching current semesters:", error);
    }
};

const RenderStudentRecords = async (RenderStudentRecords) => {
    console.log("***RenderStudentRecords was called", RenderStudentRecords);
    // clear ul element id RecordList
    document.getElementById('RecordList').innerHTML = '';
    // loop data and render to ul element id RecordList
    RenderStudentRecords.forEach((student) => {
        let li = document.createElement('li');
        
        let labelType = document.createElement('label');
        labelType.innerHTML = `${student.complianceType}`;
        li.appendChild(labelType);
        
        let labelStatus = document.createElement('label');
        labelStatus.innerHTML = `${student.complianceStatus}`;
        labelStatus.style.background = "#"+`${student.statusColor}`;
        labelStatus.style.color = "white";
        labelStatus.style.padding = "5px";
        labelStatus.style.width = "100%";
        labelStatus.style.textAlign = "center";
        li.appendChild(labelStatus);
        
        document.getElementById('RecordList').appendChild(li);
    });
    
}