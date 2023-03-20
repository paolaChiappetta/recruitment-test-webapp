import * as React from 'react';
import { useState, useEffect } from 'react';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';



export default function EmployeeForm({ dialogVisibility, isEdition, setDialogVisibility, editingEmployee, saveEmployee }) {
    const [formValue, setFormValue] = useState<Employee>(null)

    const handleChange = (e) => {
        setFormValue({ ...formValue, [e.target.name]: e.target.value });
    }

    const handleSubmit = (e) => {
        e.preventDefault();
        saveEmployee(formValue);
    }

    const handleClose = () => {
        setDialogVisibility(false);
    };

    useEffect(() => {
        if (isEdition) {
            setFormValue(editingEmployee);
        } else {
            setFormValue({
                name: '',
                lastname: '',
                value: null,
                address: '',
                phone: ''
            })
        }
    }, [dialogVisibility])


    return (
        <Dialog open={dialogVisibility} onClose={handleClose}>
            <DialogTitle>{!isEdition ? "Add new Employee" : "Edit Employee"}</DialogTitle>
            <DialogContent>
                <DialogContentText>
                    Complete or edit the fields:
                </DialogContentText>
                <TextField
                    autoFocus
                    margin="dense"
                    id="name"
                    name="name"
                    label="Name"
                    value={formValue?.name}
                    type="text"
                    fullWidth
                    variant="standard"
                    onChange={handleChange}
                    required
                />
                <TextField
                    autoFocus
                    margin="dense"
                    id="lastname"
                    name="lastname"
                    label="Lastname"
                    value={formValue?.lastname}
                    type="text"
                    fullWidth
                    variant="standard"
                    onChange={handleChange}
                    required
                />
                <TextField
                    autoFocus
                    margin="dense"
                    id="value"
                    name="value"
                    label="Value"
                    value={formValue?.value}
                    type="number"
                    fullWidth
                    variant="standard"
                    onChange={handleChange}
                    required
                />
                <TextField
                    autoFocus
                    margin="dense"
                    id="address"
                    name="address"
                    label="Address"
                    value={formValue?.address}
                    type="text"
                    fullWidth
                    variant="standard"
                    onChange={handleChange}
                />
                <TextField
                    autoFocus
                    margin="dense"
                    id="phone"
                    name="phone"
                    label="Phone"
                    value={formValue?.phone}
                    type="email"
                    fullWidth
                    variant="standard"
                    onChange={handleChange}
                />
            </DialogContent>
            <DialogActions>
                <Button onClick={handleSubmit}>Save</Button>
                <Button onClick={handleClose}>Close</Button>
            </DialogActions>
        </Dialog>
    )
}