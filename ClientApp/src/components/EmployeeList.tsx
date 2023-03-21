import * as React from 'react';
import { useState, useEffect } from 'react';
import EmployeeForm from './EmployeeForm.tsx';
import Spinner from './Spinner.tsx';
import TextField from '@mui/material/TextField';
import Tooltip from '@mui/material/Tooltip';
import Swal from 'sweetalert2';
import { ExportToCsv } from 'export-to-csv';


export default function EmployeeList() {
    const [employees, setEmployees] = useState([]);
    const [filteredEmployees, setFilteredEmployees] = useState([]);
    const [dialogVisibility, setDialogVisibility] = useState(false);
    const [isEdition, setIsEdition] = useState(false);
    const [editingEmployee, setEditingEmployee] = useState(null);
    const [filter, setFilter] = useState('');
    const [isLoading, setIsLoading] = useState(true);

    const getEmployees = async () => {
        const response = await fetch("Employees/get");

        if (response.ok) {
            const data = response.json();
            setEmployees(await data);
            setFilteredEmployees(await data);
        }
    }

    const sortByNameAndLastname = (a: any, b: any) => {
        if (a.name.toLowerCase() < b.name.toLowerCase()) {
            return -1;
        } else if (a.name.toLowerCase() > b.name.toLowerCase()) {
            return 1;
        } else {
            if (a.lastname.toLowerCase() < b.lastname.toLowerCase()) {
                return -1;
            } else if (a.lastname.toLowerCase() > b.lastname.toLowerCase()) {
                return 1;
            } else {
                return 0;
            }
        }
    }

    const onClickDeleteButton = async (employee: Employee) => {
        const text = "Employee: " + employee.name + " " + employee.lastname;

        Swal.fire({
            title: 'Are you sure you want to delete?',
            text: text,
            icon: 'question',
            confirmButtonText: 'Ok',
            showCancelButton: true,
            cancelButtonText: 'Cancel',
            confirmButtonColor: '#439c11',
            cancelButtonColor: '#d60202',
            width: '320px'
        }).then(async (result) => {
            if (result.isConfirmed) {
                const response = await fetch("Employees/delete", {
                    method: 'DELETE',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(employee.id)
                })

                if (response.ok) {
                    getEmployees();
                }
            }
        })
    }

    const onClickEditButton = async (employee: Employee) => {
        setIsEdition(true);
        setEditingEmployee(employee);
        setDialogVisibility(true);
    }

    const onClickNewEmployeeButton = async () => {
        setIsEdition(false);
        setEditingEmployee(null);
        setDialogVisibility(true);
    }

    const saveEmployee = async (employee: Employee) => {
        let response;
        if (isEdition) {
            response = await fetch("Employees/update", {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(employee)
            })
        } else {
            response = await fetch("Employees/add", {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(employee)
            })
        }

        if (response.ok) {
            setDialogVisibility(false);
            const text = !isEdition? 'The new employee was successfully added': 'The employee was successfully edited';
            Swal.fire({
                title: 'Great!',
                text: text,
                icon: 'success',
                confirmButtonText: 'OK',
                confirmButtonColor: '#1f5996',
                width: '320px'
            }).then((result) => {
                if (result.isConfirmed) {
                      getEmployees();
                }
            })            
        }
    }

    const handleChangeFilter = (e) => {
        if (e.target.value === "") {
            setFilteredEmployees(employees);
        } else {
            setFilter(e.target.value);
        }
    }

    const onClickSearchButton = (search: string) => {
        if (search !== "") {
            let filterEmployeeList: Employee[] = [];
            if (employees.length > 0) {
                employees.map((employee: Employee) => {
                    if (employee.name.toLocaleLowerCase().includes(search.toLocaleLowerCase()) ||
                        employee.lastname.toLocaleLowerCase().includes(search.toLocaleLowerCase())) {
                        filterEmployeeList.push(employee);
                    }
                })
            }
            setFilteredEmployees(filterEmployeeList);
        }
    }

    const onClickDownloadCSV = async () => {
        const config = {
            fieldSeparator: ';',
            quoteStrings: '"',
            decimalSeparator: '.',
            showLabels: true,
            showTitle: true,
            title: 'Employees CSV',
            filename: 'Employees CSV',
            useTextFile: false,
            useBom: true,
            useKeysAsHeaders: false,
            headers: ['Id', 'Name', 'Lasname', 'Value', 'Address', 'Phone']
        };

        const csvExporter = new ExportToCsv(config);

        csvExporter.generateCsv(employees);
    }

    useEffect(() => {
        getEmployees().then(() => {
            setIsLoading(false);
        });
    }, [])

    return (
        <>
            {!isLoading ?
                <div>
                    <section className="searchAndAddContainer">
                        <div className="employeeListFilterGrid">
                            <div className="employeeListFilterDiv">
                                <TextField
                                    autoFocus
                                    margin="dense"
                                    id="name"
                                    name="name"
                                    label="Search by name or lastname"
                                    type="text"
                                    variant="standard"
                                    onChange={handleChangeFilter}
                                    className="employeeListfilterInput"
                                />
                                <button className="iconButton" onClick={() => onClickSearchButton(filter)}><img className='icon' alt='' src={require('../resources/images/magnifying.png')} /></button>
                            </div>
                            <button className="employeeListNewEmployeeButton" onClick={onClickNewEmployeeButton}>Add New Employee</button>
                            <button className="employeeListDownloadButton" onClick={onClickDownloadCSV}>Download CSV</button>
                        </div>
                    </section>
                    <section className="employeeListTableContainer">
                        <table className="employeeListTable">
                            <thead>
                                <tr className="employeeListGridContainer">
                                    <th className="employeeListTableBottomBorder">Name</th>
                                    <th className="employeeListTableBottomBorder">Lastname</th>
                                    <th className="employeeListTableBottomBorder">Value</th>
                                    <th className="employeeListTableBottomBorder">Address</th>
                                    <th className="employeeListTableBottomBorder">Phone</th>
                                    <th className="employeeListTableBottomBorder">Edit</th>
                                    <th className="employeeListTableBottomBorder">Delete</th>
                                </tr>
                            </thead>
                            <tbody>
                                {filteredEmployees.length > 0 ?
                                    filteredEmployees.sort(sortByNameAndLastname).map((employee: Employee) => {
                                        return (
                                            <tr className="employeeListGridContainer" key={employee.id}>
                                                <td className="textOverflow employeeListTableBottomBorder"><Tooltip title={employee.name}><p className="textOverflow">{employee.name}</p></Tooltip></td>
                                                <td className="textOverflow employeeListTableBottomBorder"><Tooltip title={employee.lastname}><p className="textOverflow">{employee.lastname}</p></Tooltip></td>
                                                <td className="textOverflow employeeListTableBottomBorder"><Tooltip title={employee.value}><p className="textOverflow">{employee.value}</p></Tooltip></td>
                                                <td className="textOverflow employeeListTableBottomBorder"><Tooltip title={employee.address}><p className="textOverflow">{employee.address}</p></Tooltip></td>
                                                <td className="textOverflow employeeListTableBottomBorder"><Tooltip title={employee.phone}><p className="textOverflow">{employee.phone}</p></Tooltip></td>
                                                <td className="textOverflow employeeListTableBottomBorder">
                                                    <button className="iconButton" onClick={() => onClickEditButton(employee)}><img className='icon' alt='' src={require('../resources/images/edit.png')} /></button>
                                                </td>
                                                <td className="employeeListTableBottomBorder">
                                                    <button className="iconButton" onClick={() => onClickDeleteButton(employee)}><img className='icon' alt='' src={require('../resources/images/delete.png')} /></button>
                                                </td>
                                            </tr>
                                        )
                                    })
                                    :
                                    <tr><td><h3>No information available</h3></td></tr>
                                }


                            </tbody>
                        </table>
                    </section>
                    <EmployeeForm
                        dialogVisibility={dialogVisibility}
                        isEdition={isEdition}
                        setDialogVisibility={setDialogVisibility}
                        editingEmployee={editingEmployee}
                        saveEmployee={saveEmployee}
                    />
                </div>
                :
                <Spinner />
            }
        </>
    )
}