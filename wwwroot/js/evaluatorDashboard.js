console.log("Evaluator Dashboard Loaded");

// render on load
document.addEventListener('DOMContentLoaded', function () {
    fetchCurrentSemesters();
    
});


const ShowSchoolCompliance = async () => {
    const ComplianceList = document.getElementById("ComplianceList");
                ComplianceList.style.display = "grid";

                const ContollerLabel = document.getElementById("ContollerLabel");
                ContollerLabel.innerHTML = "Select Compliance";

                const ComplianceSelectedLabelRecord = document.getElementById("ComplianceSelectedLabelRecord");
                if (ComplianceSelectedLabelRecord) {
                    ComplianceSelectedLabelRecord.parentNode.removeChild(ComplianceSelectedLabelRecord);
                }


                document.getElementById("UserSelectedComplianceDiv").remove();
                const EvaluatorWindow = document.getElementById("EvaluatorWindow");
                EvaluatorWindow.style.gridTemplateColumns = "1fr 2fr";

                const UserUploadDiv = document.getElementById("UserUploadDiv");
                UserUploadDiv.remove();
}

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

const DeleteRecordHandler = async (RecordId) => {
    console.log("DeleteRecordHandler called", RecordId);
    // fetch api to delete record
    try {
        const response = await fetch(`/DeleteRecord/${RecordId}`, {
            method: "DELETE"
        });
        const data = await response.json();
        console.log("data from db", data.afterDeleteData);
        // render student records
        RenderStudentRecords(data.afterDeleteData);
        
    } catch (error) {
        console.error("Error deleting record:", error);
    }
    

}


const RenderStudentRecords = async (RenderStudentRecords) => {
    console.log("****************RenderStudentRecords was called", RenderStudentRecords);
    // clear ul element id RecordList
    document.getElementById('RecordList').innerHTML = '';
    // loop data and render to ul element id RecordList
    RenderStudentRecords.forEach((studentRecord) => {
        console.log("*********====>>studentRecord", studentRecord);
        let li = document.createElement('li');

        let button = document.createElement('button');
        button.innerHTML = "";
        button.className = "QueueDeleteBTN";
        button.addEventListener("click", () => DeleteRecordHandler(studentRecord.recordId));
        li.appendChild(button);
        
        
        let labelType = document.createElement('label');
        labelType.innerHTML = `${studentRecord.complianceType}`;
        li.appendChild(labelType);
        
        let labelStatus = document.createElement('label');
        labelStatus.innerHTML = `${studentRecord.complianceStatus}`;
        labelStatus.style.background = "#"+`${studentRecord.statusColor}`;
        labelStatus.style.color = "white";
        labelStatus.style.padding = "5px";
        labelStatus.style.width = "100%";
        labelStatus.style.textAlign = "center";
        li.appendChild(labelStatus);
        
        document.getElementById('RecordList').appendChild(li);
    });
    
}