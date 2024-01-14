import React from 'react';
import Modal from './modal.js';

/**
 * Modal used to confirm deletion of form and checking if form can be deleted
 * @param {Object} props
 * @param {boolean} props.open Optional, default = false
 * @param {Function} props.onCloseClick
 * @returns
 */
export default function DeleteFormModal(props) {
    return (
        <Modal
            open={props.open === true}
            onCloseClick={props.onCloseClick}
        >
        </Modal>
    );
}
