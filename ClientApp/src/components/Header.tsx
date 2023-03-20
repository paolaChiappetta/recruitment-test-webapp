import * as React from 'react';

export default function Header() {
    return (
        <>
            <section className="headerContainer">
                <img className='headerLogo' alt='' src={require('../resources/images/Logo2.gif')} />
                <h1>Add, edit or delete your employees...</h1>
            </section>
            <div className='line'></div>
        </>
    )
}