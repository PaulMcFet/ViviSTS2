
using System;

namespace ViviMod.ViviModCode.Mechanics;

/// <summary>
/// Stores Vivi's Trance charge and handles its rules.
/// Charge cannot increase while Trance is active.
/// </summary>
public sealed class TranceState
{
    public const int MaxCharge = 100;
    public const int ChargePerSpell = 20;
    public const int ChargePerHpLost = 5;

    public int Charge { get; private set; }

    public bool IsActive { get; private set; }

    public void Reset()
    {
        Charge = 0;
        IsActive = false;
    }

    public TranceState Clone()
    {
        return new TranceState
        {
            Charge = Charge,
            IsActive = IsActive
        };
    }

    /// <summary>
    /// Called whenever Vivi plays a Black Magic card.
    /// Returns true only when that card activates Trance.
    /// </summary>
    public bool OnBlackMagicPlayed(bool isRelicGeneratedCopy)
    {
        if (isRelicGeneratedCopy || IsActive)
            return false;

        return AddCharge(ChargePerSpell);
    }

    /// <summary>
    /// Adds charge based on actual HP lost.
    /// </summary>
    public void OnDamageTaken(int actualHpLost)
    {
        if (actualHpLost > 0)
            AddCharge(actualHpLost * ChargePerHpLost);
    }

    /// <summary>
    /// Adds charge from other cards and powers.
    /// </summary>
    public void GainCharge(int amount)
    {
        AddCharge(amount);
    }

    private bool AddCharge(int amount)
    {
        // Charge cannot increase during Trance.
        if (IsActive || amount <= 0)
            return false;

        Charge = Math.Min(MaxCharge, Charge + amount);

        if (Charge < MaxCharge)
            return false;

        IsActive = true;
        return true;
    }

    /// <summary>
    /// Spends charge when an original Black Magic card
    /// activates Doublecast or Triplecast.
    ///
    /// If less charge remains than the activation cost,
    /// all remaining charge is consumed.
    ///
    /// Zero-cost activations are allowed while Trance
    /// is active.
    /// </summary>
    public bool TrySpendActivation(
        bool isRelicGeneratedCopy,
        int normalCost = 20)
    {
        // Only original Black Magic cards can activate.
        if (!IsActive || isRelicGeneratedCopy)
            return false;

        // Prevent negative activation costs.
        int cost = Math.Max(0, normalCost);

        // A zero-cost activation is valid.
        // Trance remains active and no charge is spent.
        if (cost == 0)
            return true;

        // Cannot activate if no charge remains.
        if (Charge <= 0)
        {
            IsActive = false;
            return false;
        }

        // Spend charge, allowing partial payment.
        Charge = Math.Max(0, Charge - cost);

        // Trance ends when the gauge is depleted.
        if (Charge == 0)
            IsActive = false;

        return true;
    }
}
