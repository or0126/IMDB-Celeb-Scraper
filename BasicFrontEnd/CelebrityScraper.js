let btnResetdiv = document.querySelector("#resetbtndiv");
btnResetdiv.innerHTML = '<button class="btn" onclick="resetCelebiritiesList()">Reset</button>';

async function resetCelebiritiesList(){
    const response = await fetch(`https://localhost:44390/Celebrities/scrape`, {
    method: 'POST'
    }).then(response => {
        if(!response.ok){
            throw Error("couldnt fetch data");
        }
            
        return response.json()
    })
    .then().catch(error =>{
        console.log(error);
    });
    location.reload();
}



function deleteCelebrity(id){
    fetch(`https://localhost:44390/Celebrities/${id}`, {
    method: 'DELETE'
    }).then(response => {
        if(!response.ok){
            throw Error("couldnt fetch data");
        }
        
        return response.json()
    })
    .then(location.reload()).catch(error =>{
        console.log(error);
    });
}

function fetchCelebritiesData(){
fetch("https://localhost:44390/Celebrities")
    .then(response =>{
        if(!response.ok){
            throw Error("couldnt fetch data");
        }
        return response.json();
    })
    .then(data => {
        console.log(data);
        const html = data.map( user =>{
            return`
            <tr height="140">
                <td >${user.fullName}</td>
                <td >${parseDateFromApi(user.birthDate)}</td>
                <td >${user.gender}</td>
                <td >${user.role}</td>
                <td ><img src=${user.imageUrl} height="140"></td>
                <td>
                    <button class="btn" onclick="deleteCelebrity(${user.id})" data-Id="${user.id}">Delete</button>
                </td>
            </tr>`
        }).join('');
        document.querySelector("#table1")
        .insertAdjacentHTML("afterbegin",html);

    })
    .catch(error =>{
        console.log(error);
    });
}

function parseDateFromApi(dateFromApi){
    var localDate = new Date(dateFromApi);
    var datestring = localDate.toDateString();
    datestring= datestring.split(' ');
    return `${datestring[1]} ${datestring[2]}, ${datestring[3]}`
}


fetchCelebritiesData();