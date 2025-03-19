using System;
using System.Numerics;
using System.Linq.Expressions;

namespace CAS;

public abstract class Expressao
{
    public abstract override string ToString();
    public abstract Expressao Derivar(Simbolo x);
    public abstract Expressao Simplificar();
    public abstract Expressao Substituir(Simbolo alvo, Expressao substituto);

    public static Expressao operator +(Expressao a, Expressao b) => new Soma(a, b).Simplificar();
    public static Expressao operator -(Expressao a, Expressao b) => new Subtracao(a, b).Simplificar();
    public static Expressao operator *(Expressao a, Expressao b) => new Multiplicacao(a, b).Simplificar();
    public static Expressao operator /(Expressao a, Expressao b) => new Divisao(a, b).Simplificar();

    public static implicit operator Expressao(int v) => new Numero(v);
    public static implicit operator Expressao(string s) => new Simbolo(s);   
    public static implicit operator Expressao(Complex c) => new NumeroComplexo(c);


}

public class Numero : Expressao
{
    public int valor;
    public Numero(int v) => this.valor = v;
    public override string ToString() => valor.ToString();
    public override Expressao Derivar(Simbolo x) => new Numero(0);
    public override Expressao Simplificar() => this;
    public override Expressao Substituir(Simbolo alvo, Expressao substituto) => this;
}

public class NumeroComplexo : Expressao
{
    public Complex valor;
    public NumeroComplexo(Complex v) => this.valor = v;
    public override string ToString() => valor.ToString();
    public override Expressao Derivar(Simbolo x) => new NumeroComplexo(Complex.Zero);
    public override Expressao Simplificar() => this;
    public override Expressao Substituir(Simbolo alvo, Expressao substituto) => this;
}

public class Simbolo : Expressao
{
    string simbolo;
    public Simbolo(string s) => this.simbolo = s;
    public override string ToString() => simbolo;
    public override Expressao Derivar(Simbolo x) => 
        x.simbolo == simbolo 
            ? new Numero(1) 
            : new Numero(0);
    public override Expressao Simplificar() => this;
    public override Expressao Substituir(Simbolo alvo, Expressao substituto) => 
        simbolo == alvo.simbolo ? substituto : this;
}



public class Subtracao : Expressao
{
    Expressao a, b; // a - b
    public Subtracao(Expressao x, Expressao y)
    {
        this.a = x;
        this.b = y;
    }
    public override string ToString() => $"({a.ToString()} - {b.ToString()})";
    public override Expressao Derivar(Simbolo x) => 
        new Subtracao(a.Derivar(x), b.Derivar(x));
    public override Expressao Simplificar()
    {
       a = a.Simplificar();
       b = b.Simplificar();
       if(a is Numero na && na.valor == 0) return b;
       if(b is Numero nb && nb.valor == 0) return a;

        if(a is Numero && b is Numero)
        {
            return new Numero((a as Numero).valor - (b as Numero).valor);
        }
        return this;
    }
    public override Expressao Substituir(Simbolo alvo, Expressao substituto) => 
        new Subtracao(a.Substituir(alvo, substituto), b.Substituir(alvo, substituto)).Simplificar();
}

public class Soma : Expressao
{
    Expressao a, b; // a + b
    public Soma(Expressao x, Expressao y)
    {
        this.a = x;
        this.b = y;
    }
    public override string ToString() => $"({a.ToString()} + {b.ToString()})";

    public override Expressao Derivar(Simbolo x) =>
        a.Derivar(x) + b.Derivar(x);

    public override Expressao Simplificar(){
       a = a.Simplificar();
       b = b.Simplificar();
       if(a is Numero na && na.valor == 0) return b;
       if(b is Numero nb && nb.valor == 0) return a;
       if (a is Numero && b is Numero)
       {
          return new Numero((a as Numero).valor + (b as Numero).valor);
       }
       return this;
    }

    public override Expressao Substituir(Simbolo alvo, Expressao substituto) => 
        new Soma(a.Substituir(alvo, substituto), b.Substituir(alvo, substituto)).Simplificar();           
}

public class Multiplicacao : Expressao
{
    Expressao a, b; // a * b
    public Multiplicacao(Expressao x, Expressao y)
    {
        this.a = x;
        this.b = y;
    }
    public override string ToString() => $"({a.ToString()} * {b.ToString()})";

    public override Expressao Derivar(Simbolo x) =>
        a.Derivar(x) * b + a * b.Derivar(x);

    public override Expressao Simplificar()
    {
        a = a.Simplificar();
        b = b.Simplificar();

        if (a is Numero na && na.valor == 0 || b is Numero nb && nb.valor == 0)
        {
            return new Numero(0);
        }

        if (a is Numero na1 && na1.valor == 1) return b;
        if (b is Numero nb1 && nb1.valor == 1) return a;

        if (a is Numero && b is Numero)
        {
            return new Numero((a as Numero).valor * (b as Numero).valor);
        }

        return this;
    }

    public override Expressao Substituir(Simbolo alvo, Expressao substituto) => 
        new Multiplicacao(a.Substituir(alvo, substituto), b.Substituir(alvo, substituto)).Simplificar();
}


public class Divisao : Expressao
{
    Expressao a, b; // a / b
    public Divisao(Expressao x, Expressao y)
    {
        this.a = x;
        this.b = y;
    }
    public override string ToString() => $"({a.ToString()} / {b.ToString()})";
    public override Expressao Derivar(Simbolo x) =>
        new Divisao(
            new Subtracao(
                new Multiplicacao(a.Derivar(x), b), 
                new Multiplicacao(a, b.Derivar(x))),
            new Multiplicacao(b, b));
    public override Expressao Simplificar()
    {
        a = a.Simplificar();
        b = b.Simplificar();

        if (b is Numero nb && nb.valor == 0)
        {
            throw new DivideByZeroException("Cannot divide by zero.");
        }

        if (b is Numero nb1 && nb1.valor == 1) return a;

        if (a is Numero && b is Numero)
        {
            return new Numero((a as Numero).valor / (b as Numero).valor);
        }

        return this;
    }
    public override Expressao Substituir(Simbolo alvo, Expressao substituto) => 
        new Divisao(a.Substituir(alvo, substituto), b.Substituir(alvo, substituto)).Simplificar();
}
