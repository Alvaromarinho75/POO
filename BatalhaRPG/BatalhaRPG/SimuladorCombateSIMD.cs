using System;
using System.Numerics;

public class SimuladorCombateSIMD
{
    private static Random gerador = new Random(42);

    public static int CalcularDanoVetorizado(ExercitoSIMD atacantes, ExercitoSIMD defensores)
    {
        int tamanho = atacantes.Ataques.Length;
        int tamanhoVetor = Vector<int>.Count;
        int danoTotal = 0;

        // Pré-calcula valores aleatórios para crítico
        int[] randomCrit = new int[tamanho];
        for (int i = 0; i < tamanho; i++)
            randomCrit[i] = gerador.Next(0, 100);

        // Conversão de bool para int (-1 para true, 0 para false)
        int[] vivosAtacante = new int[tamanho];
        int[] vivosDefensor = new int[tamanho];
        for (int i = 0; i < tamanho; i++)
        {
            vivosAtacante[i] = atacantes.Vivos[i] ? -1 : 0;
            vivosDefensor[i] = defensores.Vivos[i] ? -1 : 0;
        }

        for (int i = 0; i <= tamanho - tamanhoVetor; i += tamanhoVetor)
        {
            var vAtaque = new Vector<int>(atacantes.Ataques, i);
            var vDefesa = new Vector<int>(defensores.Defesas, i);
            var vChanceCrit = new Vector<int>(atacantes.ChancesCritico, i);
            var vMultCrit = new Vector<int>(atacantes.MultCriticos, i);
            var vVivoAtacante = new Vector<int>(vivosAtacante, i);
            var vVivoDefensor = new Vector<int>(vivosDefensor, i);
            var vVivos = Vector.BitwiseAnd(vVivoAtacante, vVivoDefensor);
            var vRandom = new Vector<int>(randomCrit, i);

            // Dano base = Ataque - Defesa (mínimo 1)
            var vDanoBase = Vector.Max(Vector.Subtract(vAtaque, vDefesa), Vector<int>.One);

            // Crítico: random < chance?
            var mascaraCritico = Vector.LessThan(vRandom, vChanceCrit);

            // Aplica multiplicador de crítico
            var vDanoCrit = Vector.Divide(Vector.Multiply(vDanoBase, vMultCrit), new Vector<int>(100));
            var vDanoFinal = Vector.ConditionalSelect(mascaraCritico, vDanoCrit, vDanoBase);

            // Zera dano de mortos
            vDanoFinal = Vector.ConditionalSelect(vVivos, vDanoFinal, Vector<int>.Zero);

            // Soma os danos deste vetor
            danoTotal += Vector.Dot(vDanoFinal, Vector<int>.One);
        }

        // Processa o resto (caso o tamanho não seja múltiplo do vetor)
        for (int i = (tamanho / tamanhoVetor) * tamanhoVetor; i < tamanho; i++)
        {
            if (atacantes.Vivos[i] && defensores.Vivos[i])
            {
                int danoBase = Math.Max(1, atacantes.Ataques[i] - defensores.Defesas[i]);
                bool ehCritico = randomCrit[i] < atacantes.ChancesCritico[i];
                if (ehCritico)
                    danoBase = (danoBase * atacantes.MultCriticos[i]) / 100;
                danoTotal += danoBase;
            }
        }

        return danoTotal;
    }
} 