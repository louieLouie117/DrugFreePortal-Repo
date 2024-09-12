console.log("adminDashboard.js loaded");

document.addEventListener('DOMContentLoaded', () => {
  fetchAllUsers();
  getSchools();
  getSchoolsForDeanReg();
  getSemesters();
  loadComplianceTypes();
});
window.onload = function () {}; // loads after all the elements are loaded


// arrow function to fetch all users from the database
const fetchAllUsers = async () => {
    fetch("/GetUsers")
    .then(response => response.json())
    .then(data => {
        console.log("data from db", data);
        console.log("users list", data.usersList);

        RenderAllUsers(data.usersList);
    });
};






const RenderAllUsers = (users) => {
const table = document.getElementById('usersList'); // Assuming you have a table with id 'usersList'
    // Clear the usersList id table
    table.innerHTML = "";

    users.forEach(user => {
        // Map accountType values to descriptive strings
        switch (user.accountType) {
            case 0:
                user.accountType = 'Admin';
                break;
            case 1:
                user.accountType = 'Dean';
                break;
            case 2:
                user.accountType = 'Student';
                break;
            case 3:
                user.accountType = 'Evaluator';
                break;
            default:
                user.accountType = 'Unknown';
        }

        // Create a new table row
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${user.userId}</td>
            <td>${user.accountType}</td>
            <td>${user.schoolId}</td>
            <td>${user.school}</td>
            <td>${user.studentId}</td>
            <td>${user.firstName}</td>
            <td>${user.lastName}</td>
            <td>${user.email}</td>
            <td><button class="hidden"id="${user.userId}" onclick="DeleteUserHandler(${user.userId})">Delete</button></td>
        `;

        // Append the row to the table
        table.appendChild(row);
    });
};




const loadComplianceTypes = () => {
    fetch('/GetComplianceTypes')
        .then(response => {
            if (response.ok) {
                // Handle successful response
                return response.json();
            } else {
                // Handle error response
                throw new Error('Failed to get compliance types');
            }
        })
        .then(data => {
            // Handle data from backend
            console.log('Data from backend on load:', data);
            // Call a function to handle the data
            ComplianceTypeList(data);
        })
        .catch(error => {
            // Handle network error
            console.error('Network error:', error);
        });
};





const ComplianceTypeList = (data) => {
    console.log("data to loop", data.data);
    const complianceTypeList = document.getElementById("ComplianceTypeList");
    complianceTypeList.innerHTML = "";
    data.data.forEach(compliance => {
        console.log("compliance", compliance);
     
       
        // Create table body
        let row = `
            <tr>
            <td>${compliance.complianceTypeId}</td>
            <td>${compliance.name}</td>
            <td>${compliance.school}</td>
            <td>${compliance.idFromSchool}</td>
            <td>${compliance.details}</td>
            <td><button id="${compliance.complianceTypeId}" onclick="EditComplianceHandler('${compliance.school}', '${compliance.idFromSchool}', ${compliance.complianceTypeId}, '${compliance.name}', '${compliance.details}')">Edit</button></td>
            </tr>
        `;

        // Append the table body to the complianceTypeList
        complianceTypeList.innerHTML += row;
    });
};




// 
const CreateComplianceHandler = (event) => {
    event.preventDefault();

    const SchoolsSelector = document.getElementById("SchoolsSelector").value;
    const IdFromSchool = document.getElementById("IdFromSchool").value;
    const complianceName = document.getElementById("ComplianceName").value;
    const complianceDetails = document.getElementById("ComplianceDetails").value;
    const EditComplianceId = document.getElementById("EditComplianceId").value;


    // check if complianceName has special characters
    if (complianceName.match(/[^a-zA-Z0-9 _\-]/)) {
        alert("Compliance name cannot contain special characters");
        return;
    }
  
    const dataCreateCompliance = {
        school: SchoolsSelector,
        idFromSchool: IdFromSchool,
        name: complianceName,
        details: complianceDetails
    };

    const dataToEdit = {
        school: SchoolsSelector,
        idFromSchool: IdFromSchool,
        name: complianceName,
        details: complianceDetails,
        complianceTypeId: EditComplianceId,
       
    };

    //if button = Create Compliance
    if (event.target.innerText === "Create Compliance") {

        
        console.log(dataCreateCompliance);
        if (SchoolsSelector === "Select School") {
            alert("Please select a school");
            return;
        }
        fetch('/AddCompliance', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(dataCreateCompliance)
        })
            .then(response => {
                if (response.ok) {
                    // Handle successful response
                    console.log('Compliance created successfully');
                    loadComplianceTypes()
                    return;
                } else {
                    // Handle error response
                    console.error('Failed to create compliance');
                    throw new Error('Failed to create compliance');
                }
            })
            .then(data => {
                // Handle data from backend
                console.log('Data from backend after submitting:', data);
                // Call a function to handle the data

            })
            .catch(error => {
                // Handle network error
                console.error('Network error:', error);
            });
    }

    //Edit Compliance
    if (event.target.innerText === "Save Changes") {

        console.log(dataToEdit);
    
        fetch('/EditCompliance', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(dataToEdit)
        })
            .then(response => {
                if (response.ok) {
                    // Handle successful response
                    console.log('Compliance edited successfully');
                    loadComplianceTypes()
                return;
                } else {
                    // Handle error response
                    console.error('Failed to create compliance');
                    throw new Error('Failed to create compliance');
                }
            })
            .then(data => {
                // Handle data from backend
                console.log('Data from backend after submitting:', data);
                // Call a function to handle the data
            })
            .catch(error => {
                // Handle network error
                console.error('Network error:', error);
            });
    }

};

const EditComplianceHandler = (school, schoolId, complianceTypeId, name, details) => {
    console.log('Editing compliance with id:', complianceTypeId, name, details);


    let ComplianceSubmitBTN = document.getElementById("ComplianceSubmitBTN");
    ComplianceSubmitBTN.innerText = "Save Changes";

    let schoolSelector = document.getElementById("SchoolsSelector");
    schoolSelector.value = school;

    let idFromSchool = document.getElementById("IdFromSchool");
    idFromSchool.value = schoolId;

    // place the name in the input field
    let complianceNameInput = document.getElementById("ComplianceName");
    complianceNameInput.value = name;

    let complianceDetailsInput = document.getElementById("ComplianceDetails"); // Move the declaration here
    complianceDetailsInput.value = details; 

    let editComplianceIdInput = document.getElementById("EditComplianceId");
    editComplianceIdInput.value = complianceTypeId;
    editComplianceIdInput.style.display = "block";
    

    return
}


// -------------------Schools-------------------



/*
const deleteSchool = (schoolId) => {
    console.log('Deleting school with id:', schoolId);
    fetch('/DeleteSchool', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ schoolId })
    })
        .then(response => response.json())
        .then(data => {
            // Handle success response
            console.log('School deleted successfully', data);
            getSchools();
        })
        .catch(error => {
            // Handle error response
            console.error('Failed to delete school', error);
        });

}
**/

const deleteSchoolHandler = (e, schoolId) => {
    let labelName = document.getElementById('schoolName' + schoolId);

    if(e.target.innerHTML === "Delete"){
    
        let deleteSchoolDiv = document.getElementById('deleteSchoolDiv' + schoolId);
        deleteSchoolDiv.style.display = 'block';
        return;
    }
    if(e.target.innerHTML === "Yes"){
        let password = document.getElementById('password' + schoolId);
        password.style.display = 'block';        
        e.target.innerHTML = "Confirm";
        labelName.innerHTML = "Please write down the school id before deleting.";
        return;

    }

    if(e.target.innerHTML === "No"){
        let deleteSchoolDiv = document.getElementById('deleteSchoolDiv' + schoolId);
        deleteSchoolDiv.style.display = 'none';
        return;
    }

    if (e.target.innerHTML === "Confirm") {
        let password = document.getElementById('password' + schoolId);
    
        //check if password is empty
        if (password.value === "") {
            alert("Please enter the delete password");
            return;
        }


            const data = {
                SchoolId: schoolId,
                dean: "schoolDean",
                name: password.value,
                address: "schoolAddress",
                city: "schoolCity",
                state: "schoolState",
                zipCode: "schoolZipCode",
                onSiteDate: "schoolOnsiteDate",
                
            };

            fetch('/DeleteSchool', {
                method: 'Delete',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            })
                .then(response => response.json())
                .then(data => {
                    // Handle success response
                    console.log('School deleted successfully', data);
                    alert(data.message);
                    getSchools();
                })
                .catch(error => {
                    // Handle error response
                    console.error('Failed to delete school', error);
                });
        // if (password.value === "delete123") {

        // }
    }

}

const getSchools = () => {
    fetch('/GetAllSchools', {
        method: 'GET'
    })
        .then(response => response.json())
        .then(data => {
            // Handle success response
            console.log("schools--------", data.schoolData);
            RenderSchoolsAsUl(data.schoolData);
            RenderSchoolsOptions(data.schoolData);

        })
        .catch(error => {
            // Handle error response
            console.log(error);
        });
};

function RenderSchoolsAsUl(schools) {
    const allSchools = document.getElementById('AllSchools');
    allSchools.innerHTML = '';

    schools.forEach(school => {
        const li = document.createElement('li');
        const idLabel = document.createElement('label');
        idLabel.textContent = "ID: " + school.schoolId;
        li.appendChild(idLabel);


        const label = document.createElement('label');
        label.textContent =  school.name;
        li.appendChild(label);
        
        // Create a div to hold the buttons
        const buttonDiv = document.createElement('div');
        buttonDiv.style.display = 'none';
        buttonDiv.id = 'deleteSchoolDiv' + school.schoolId;

        //create label permanently to delete school
        const deleteLabel = document.createElement('label');
        deleteLabel.textContent = 'Permanently Delete School? This action can not be undo all data will be lost.';
        deleteLabel.style.color = 'red';
        deleteLabel.id = "schoolName" + school.schoolId;

        buttonDiv.appendChild(deleteLabel);
        
        // Create a hidden input element with placeholder password
        const hiddenInput = document.createElement('input');
        hiddenInput.type = 'password';
        hiddenInput.placeholder = 'Enter delete password';
        hiddenInput.style.display = 'none';
        hiddenInput.id = 'password' + school.schoolId;
        buttonDiv.appendChild(hiddenInput);
        
        // Create the 'Yes' button
        const yesButton = document.createElement('button');
        yesButton.textContent = 'Yes';
        yesButton.addEventListener('click', (event) => {
            // call the delete school function
            deleteSchoolHandler(event, school.schoolId);
        });
        buttonDiv.appendChild(yesButton);
        
        // Create the 'No' button
        const noButton = document.createElement('button');
        noButton.textContent = 'No';
        noButton.addEventListener('click', (event) => {
            // call the delete school function
            deleteSchoolHandler(event, school.schoolId);
        });
        buttonDiv.appendChild(noButton);
        
        // Append the button div to the li
        li.appendChild(buttonDiv);
        
        // generate delete button
        const deleteButton = document.createElement('button');
        deleteButton.textContent = 'Delete';
        // add an event listener to the delete button
        deleteButton.addEventListener('click', (event) => {
            // call the delete school function
            deleteSchoolHandler(event, school.schoolId);
        });
        // append the delete button to the li
        li.appendChild(deleteButton);
        allSchools.appendChild(li);
    });
}

function RenderSchoolsOptions(schools) {
    const SchoolsSelector = document.getElementById('SchoolsSelector');
    SchoolsSelector.innerHTML = '<option>Select School</option>';

    schools.forEach(school => {
        console.log("school option", school.schoolId);

        const option = document.createElement('option');
        option.value = school.name; // It's common to use the id as the value
        option.textContent = school.name;
        option.id = school.schoolId;
        SchoolsSelector.appendChild(option);
    });

    // Add the event listener to the select element
    SchoolsSelector.addEventListener('change', addIdToInput);
}

function addIdToInput(event) {
    // The value of the selected option is the schoolId
    const selectedSchoolId = event.target.options[event.target.selectedIndex].id;
    console.log(selectedSchoolId);
    document.getElementById('IdFromSchool').value = selectedSchoolId;
}



const getSchoolsForDeanReg = () => {
    fetch('/GetAllSchools', {
        method: 'GET'
    })
        .then(response => response.json())
        .then(data => {
            // Handle success response
            console.log("schools list--------", data.schoolData);
            RenderSchoolsOptionsForDeanReg(data.schoolData);

        })
        .catch(error => {
            // Handle error response
            console.log(error);
        });
};

function RenderSchoolsOptionsForDeanReg(schools) {
    const DeanSchoolSelector = document.getElementById('DeanSchoolSelector');
    DeanSchoolSelector.innerHTML = '<option>Select School</option>';

    schools.forEach(school => {
        console.log("school option", school.schoolId);

        const option = document.createElement('option');
        option.value = school.name; // It's common to use the id as the value
        option.textContent = school.name;
        option.id = school.schoolId;
        DeanSchoolSelector.appendChild(option);
    });

    // Add the event listener to the select element
    DeanSchoolSelector.addEventListener('change', addIdToInputStudentReg);
}