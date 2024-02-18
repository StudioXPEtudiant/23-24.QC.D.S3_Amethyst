using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CapsuleControl : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private LayerMask trackLayer;
    
    
    private Rigidbody _rigidbody;
    private Vector3 _trackNormal = Vector3.up;
    private Vector3 _newTrackNormal = Vector3.up;
    private Transform _referenceTransform;
    private float _speedMultiplier = 200;

    public void SetReferenceTransform(Transform reference)
    {
        _referenceTransform = reference;
    }

   
    void Update()
    {
        UpdateReference();
        
        Vector3 speedVector = Vector3.zero;
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        if (verticalAxis != 0)
        {
            if (verticalAxis > 0)
                speedVector += _referenceTransform.forward;
            else if (verticalAxis < 0)
                speedVector -= _referenceTransform.forward;
        }
        
        if (horizontalAxis != 0)
        {
            if (horizontalAxis > 0)
                speedVector += _referenceTransform.right;
            else if (horizontalAxis < 0)
                speedVector -= _referenceTransform.right;
        }
        
        var diffNormal = _newTrackNormal - _trackNormal;
        _trackNormal += diffNormal * Time.deltaTime;
        _rigidbody.velocity -= _trackNormal * (Physics2D.gravity.magnitude * Time.deltaTime);

        if (speedVector != Vector3.zero)
            _rigidbody.AddForce(speedVector.normalized * (_speedMultiplier * speed * Time.deltaTime));
    }

    private void OnCollisionStay(Collision other)
    {
        if (trackLayer != (trackLayer | (1 << other.gameObject.layer))) return;
        _newTrackNormal = other.contacts.Length > 0 ? other.contacts[0].normal : Vector3.up;
    }

    private void UpdateReference()
    {
        var forward = _referenceTransform.forward;
        forward -= (forward - _rigidbody.velocity) * Time.deltaTime;
        
        _referenceTransform.LookAt(transform.position + forward, _trackNormal);
    }

    private void OnDrawGizmos()
    {
        if (_referenceTransform == null)
            return;
        
        Debug.DrawRay(_referenceTransform.position, _referenceTransform.forward * 4, Color.green);
    }
}