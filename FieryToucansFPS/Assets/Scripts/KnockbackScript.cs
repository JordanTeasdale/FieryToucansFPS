using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockbackScript : MonoBehaviour {
    [SerializeField] float knockbackTime;
    [SerializeField] int IFramesKnockback;

    [Range(0, 5f)] [SerializeField] float changeInX;
    [Range(0, 5f)] [SerializeField] float changeInY;
    [Range(0, 5f)] [SerializeField] float changeInZ;

    [Range(0, 2f)] [SerializeField] float YDirectionScalar;
    [Range(0, 2f)] [SerializeField] float XDirectionScalar;
    [Range(0, 2f)] [SerializeField] float ZDirectionScalar;


    public IEnumerator Knockback(float _knockbackForce, GameObject _objectAffected, Collision _collision) {

        if (_objectAffected.CompareTag("Player") || _objectAffected.CompareTag("Enemy")) {
            Vector3 collisionPoint = new Vector3(_collision.GetContact(0).point.x, 0, _collision.GetContact(0).point.z) - Vector3.zero;   
            float collisionAngle = Vector3.Angle(collisionPoint.normalized, _objectAffected.transform.forward);
            if (_objectAffected.CompareTag("Player")) {

                _objectAffected.GetComponent<PlayerController>().enabled = false;

                Effect(_objectAffected, collisionAngle, _knockbackForce);
                Debug.Log(collisionAngle);
              
                _objectAffected.GetComponent<PlayerController>().enabled = true;
                yield return new WaitForSeconds(IFramesKnockback);

            } else {

                _objectAffected.TryGetComponent<NavMeshAgent>(out NavMeshAgent enemyMovement);

                enemyMovement.enabled = false;

                Effect(_objectAffected, collisionAngle, _knockbackForce);
                Debug.Log(collisionAngle);

                enemyMovement.enabled = true;
                yield return new WaitForSeconds(knockbackTime);

            }


        }


    }


    private void Effect(GameObject _objectAffected, float _collisionAngle, float _knockbackForce) {
        if ((_collisionAngle >= 0f && _collisionAngle <= 22.5f) || (_collisionAngle >= -22.5f && _collisionAngle < 0f)) {

            if (_collisionAngle >= 0f && _collisionAngle <= 22.5f)
                _objectAffected.transform.localPosition -= new Vector3(Mathf.Lerp(_objectAffected.transform.localPosition.x, changeInX * XDirectionScalar, knockbackTime),
                                                                    Mathf.Lerp(_objectAffected.transform.localPosition.y, changeInY * YDirectionScalar, knockbackTime),
                                                                        Mathf.Lerp(_objectAffected.transform.localPosition.x, 2f * ZDirectionScalar, knockbackTime)) * _knockbackForce;
            else
                _objectAffected.transform.localPosition -= new Vector3(Mathf.Lerp(_objectAffected.transform.localPosition.x, -changeInX * XDirectionScalar, knockbackTime),
                                                                   Mathf.Lerp(_objectAffected.transform.localPosition.y, -changeInY * YDirectionScalar, knockbackTime),
                                                                       Mathf.Lerp(_objectAffected.transform.localPosition.x, 2f * ZDirectionScalar, knockbackTime)) * _knockbackForce;
        }
        if ((_collisionAngle >= 22.6f && _collisionAngle <= 45f) || (_collisionAngle >= -45f && _collisionAngle <= -22.6f)) {

            if (_collisionAngle >= 22.6f && _collisionAngle <= 45f)
                _objectAffected.transform.localPosition -= new Vector3(Mathf.Lerp(_objectAffected.transform.localPosition.x, changeInX * XDirectionScalar, knockbackTime),
                                                                    Mathf.Lerp(_objectAffected.transform.localPosition.y, -changeInY * YDirectionScalar, knockbackTime),
                                                                        Mathf.Lerp(_objectAffected.transform.localPosition.x, 2f * ZDirectionScalar, knockbackTime)) * _knockbackForce;
            else
                _objectAffected.transform.localPosition -= new Vector3(Mathf.Lerp(_objectAffected.transform.position.x, -changeInX * XDirectionScalar, knockbackTime),
                                                                   Mathf.Lerp(_objectAffected.transform.position.y, -2f * YDirectionScalar, knockbackTime),
                                                                       Mathf.Lerp(_objectAffected.transform.position.x, 2f * ZDirectionScalar, knockbackTime)) * _knockbackForce;
        }
        if ((_collisionAngle >= 45.1f && _collisionAngle <= 67.5f) || (_collisionAngle >= -67.5f && _collisionAngle <= -45.1f)) {

            if (_collisionAngle >= 45.1f && _collisionAngle <= 67.5f)
                _objectAffected.transform.localPosition -= new Vector3(Mathf.Lerp(_objectAffected.transform.localPosition.x, changeInX * XDirectionScalar, knockbackTime),
                                                                    Mathf.Lerp(_objectAffected.transform.localPosition.y, -2f * YDirectionScalar, knockbackTime),
                                                                        Mathf.Lerp(_objectAffected.transform.localPosition.x, 1f * ZDirectionScalar, knockbackTime)) * _knockbackForce;
            else
                _objectAffected.transform.localPosition -= new Vector3(Mathf.Lerp(_objectAffected.transform.localPosition.x, -changeInX * XDirectionScalar, knockbackTime),
                                                                   Mathf.Lerp(_objectAffected.transform.localPosition.y, -2f * YDirectionScalar, knockbackTime),
                                                                       Mathf.Lerp(_objectAffected.transform.localPosition.x, 2f * ZDirectionScalar, knockbackTime)) * _knockbackForce;
        }
        if ((_collisionAngle >= 67.6 && _collisionAngle <= 90f) || (_collisionAngle >= -90f && _collisionAngle <= -67.6f)) {

            if (_collisionAngle >= 67.6 && _collisionAngle <= 90f)
                _objectAffected.transform.localPosition -= new Vector3(Mathf.Lerp(_objectAffected.transform.localPosition.x, changeInX * XDirectionScalar, knockbackTime),
                                                                    Mathf.Lerp(_objectAffected.transform.localPosition.y, -2f * YDirectionScalar, knockbackTime),
                                                                        Mathf.Lerp(_objectAffected.transform.localPosition.x, 2f * ZDirectionScalar, knockbackTime)) * _knockbackForce;
            else
                _objectAffected.transform.localPosition -= new Vector3(Mathf.Lerp(_objectAffected.transform.localPosition.x, -changeInX * XDirectionScalar, knockbackTime),
                                                                   Mathf.Lerp(_objectAffected.transform.localPosition.y, -2f * YDirectionScalar, knockbackTime),
                                                                       Mathf.Lerp(_objectAffected.transform.localPosition.x, 2f * ZDirectionScalar, knockbackTime)) * _knockbackForce;
        }
        if ((_collisionAngle >= 90.1f && _collisionAngle <= 112.5f) || (_collisionAngle >= -112.5f && _collisionAngle <= -90.1f)) {

            if (_collisionAngle >= 90.1f && _collisionAngle <= 112.5f)
                _objectAffected.transform.localPosition -= new Vector3(Mathf.Lerp(_objectAffected.transform.localPosition.x, changeInX * XDirectionScalar, knockbackTime),
                                                                    Mathf.Lerp(_objectAffected.transform.localPosition.y, -2f * YDirectionScalar, knockbackTime),
                                                                        Mathf.Lerp(_objectAffected.transform.localPosition.x, -2f * ZDirectionScalar, knockbackTime)) * _knockbackForce;
            else
                _objectAffected.transform.localPosition -= new Vector3(Mathf.Lerp(_objectAffected.transform.localPosition.x, -changeInX * XDirectionScalar, knockbackTime),
                                                                   Mathf.Lerp(_objectAffected.transform.localPosition.y, -2f * YDirectionScalar, knockbackTime),
                                                                       Mathf.Lerp(_objectAffected.transform.localPosition.x, -2f * ZDirectionScalar, knockbackTime)) * _knockbackForce;
        }
        if ((_collisionAngle >= 112.6f && _collisionAngle <= 135f) || (_collisionAngle >= -135f && _collisionAngle <= -112.6f)) {

            if (_collisionAngle >= 112.6f && _collisionAngle <= 135f)
                _objectAffected.transform.localPosition -= new Vector3(Mathf.Lerp(_objectAffected.transform.localPosition.x, changeInX * XDirectionScalar, knockbackTime),
                                                                    Mathf.Lerp(_objectAffected.transform.localPosition.y, -2f * YDirectionScalar, knockbackTime),
                                                                        Mathf.Lerp(_objectAffected.transform.localPosition.x, -2f * ZDirectionScalar, knockbackTime)) * _knockbackForce;
            else
                _objectAffected.transform.localPosition -= new Vector3(Mathf.Lerp(_objectAffected.transform.localPosition.x, -changeInX * XDirectionScalar, knockbackTime),
                                                                   Mathf.Lerp(_objectAffected.transform.localPosition.y, -2f * YDirectionScalar, knockbackTime),
                                                                       Mathf.Lerp(_objectAffected.transform.localPosition.x, -2f * ZDirectionScalar, knockbackTime)) * _knockbackForce;
        }
        if ((_collisionAngle >= 135.1f && _collisionAngle <= 157.5f) || (_collisionAngle >= -157.5f && _collisionAngle <= -135.1f)) {

            if (_collisionAngle >= 135.1f && _collisionAngle <= 157.5f)
                _objectAffected.transform.localPosition -= new Vector3(Mathf.Lerp(_objectAffected.transform.localPosition.x, changeInX * XDirectionScalar, knockbackTime),
                                                                    Mathf.Lerp(_objectAffected.transform.localPosition.y, -2f * YDirectionScalar, knockbackTime),
                                                                        Mathf.Lerp(_objectAffected.transform.localPosition.x, -2f * ZDirectionScalar, knockbackTime)) * _knockbackForce;
            else
                _objectAffected.transform.localPosition -= new Vector3(Mathf.Lerp(_objectAffected.transform.localPosition.x, -changeInX * XDirectionScalar, knockbackTime),
                                                                   Mathf.Lerp(_objectAffected.transform.localPosition.y, -2f * YDirectionScalar, knockbackTime),
                                                                       Mathf.Lerp(_objectAffected.transform.localPosition.x, -2f * ZDirectionScalar, knockbackTime)) * _knockbackForce;
        }
        if ((_collisionAngle >= 157.6f && _collisionAngle <= 180f) || (_collisionAngle >= -180f && _collisionAngle <= -157.6)) {

            if (_collisionAngle >= 157.6f && _collisionAngle <= 180f)
                _objectAffected.transform.localPosition -= new Vector3(Mathf.Lerp(_objectAffected.transform.localPosition.x, changeInX * XDirectionScalar, knockbackTime),
                                                                    Mathf.Lerp(_objectAffected.transform.localPosition.y, -2f * YDirectionScalar, knockbackTime),
                                                                        Mathf.Lerp(_objectAffected.transform.localPosition.x, -2f * ZDirectionScalar, knockbackTime)) * _knockbackForce;
            else
                _objectAffected.transform.localPosition -= new Vector3(Mathf.Lerp(_objectAffected.transform.localPosition.x, -changeInX * XDirectionScalar, knockbackTime),
                                                                   Mathf.Lerp(_objectAffected.transform.localPosition.y, -2f * YDirectionScalar, knockbackTime),
                                                                       Mathf.Lerp(_objectAffected.transform.localPosition.x, -2f * ZDirectionScalar, knockbackTime)) * _knockbackForce;
        }

    }

}
