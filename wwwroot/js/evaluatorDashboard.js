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