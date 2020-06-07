using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenMovement : MonoBehaviour
{
    // Start is called before the first frame update

    float dirX, dirY;
    public float moveSpeed = 5f;
    Rigidbody2D rb;
    public UIManager uIManager;
    public Token token;
    public TextMesh textoEncima;
    float tiempo;

    //Metodo Start, busca el canvas y le coloca el nombre del usuario encima al token
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        uIManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
        textoEncima.text = token.Perfil;
    }

    // Update is called once per frame
    // Obtiene la informacion del teclado de si pulsas wasd o las flechas de direccion y las guarda en variables
    void Update()
    {
        dirX = Input.GetAxis("Horizontal");
        dirY = Input.GetAxis("Vertical");
    }

    //metodo fixedupdate que mueve el token segun las coordenadas obtenidas en el update y que envia estas al servidor mediante el metodo del UIManager, sendInfoToken
    private void FixedUpdate()
    {
        if (string.Equals(uIManager.carga.assigned, "true")&&string.Equals(uIManager.perfil.Nombre,token.Perfil))
        {
            rb.velocity = new Vector2(dirX * moveSpeed, dirY * moveSpeed);
            token.Y = this.gameObject.transform.position.y;
            token.X = this.gameObject.transform.position.x;
            if (tiempo > 0.1)
            {
                if (dirX != 0 || dirY != 0)
                {
                    uIManager.sendInfoToken(token);
                }
                tiempo = 0;
            }
            tiempo = tiempo + Time.deltaTime;

        }
        else
        {
            this.gameObject.transform.position = new Vector2(token.X, token.Y);
        }
        
    }
}
