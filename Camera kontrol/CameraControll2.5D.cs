using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
   public Transform target;  // Kameranın takip edeceği hedef (karakter)
    public float smoothSpeed = 5f;  // Kamera takibindeki yumuşaklık hızı
    public Vector3 offset = new Vector3(0f, 10f, -7f);  // Kamera ile hedef arasındaki başlangıç mesafesi

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Hedef (target) atanmamış!");
            return;
        }

        // Hedefin pozisyonunu takip et
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Hedefi takip eden bir kamera, hedefin dönme hareketini takip et
        transform.LookAt(target);

        // Kameranın yatay eksende dönmesini sağla (karakterin bakış yönüne göre)
        RotateWithMouse();
    }

    void RotateWithMouse()
    {
        // Kameranın yatay eksende dönmesini sağla (karakterin bakış yönüne göre)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 lookAtPoint = new Vector3(hit.point.x, target.position.y, hit.point.z);
            target.LookAt(lookAtPoint);
        }
    }
}
